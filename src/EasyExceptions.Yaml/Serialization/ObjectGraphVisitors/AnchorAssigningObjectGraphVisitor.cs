using System;
using System.Collections.Generic;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization.ObjectGraphVisitors
{
    public sealed class AnchorAssigningObjectGraphVisitor : ChainedObjectGraphVisitor
    {
        private readonly IEventEmitter eventEmitter;
        private readonly IAliasProvider aliasProvider;
        private readonly HashSet<AnchorName> emittedAliases = new HashSet<AnchorName>();

        public AnchorAssigningObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor, IEventEmitter eventEmitter, IAliasProvider aliasProvider)
            : base(nextVisitor)
        {
            this.eventEmitter = eventEmitter;
            this.aliasProvider = aliasProvider;
        }

        public override bool Enter(IObjectDescriptor value, IEmitter context)
        {
            if (value.Value != null)
            {
                var alias = aliasProvider.GetAlias(value.Value);
                if (!alias.IsEmpty && !emittedAliases.Add(alias))
                {
                    var aliasEventInfo = new AliasEventInfo(value, alias);
                    eventEmitter.Emit(aliasEventInfo, context);
                    return false;
                }
            }
            return base.Enter(value, context);
        }

        public override void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, IEmitter context)
        {
            var anchor = aliasProvider.GetAlias(mapping.NonNullValue());
            eventEmitter.Emit(new MappingStartEventInfo(mapping) { Anchor = anchor }, context);
        }

        public override void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, IEmitter context)
        {
            var anchor = aliasProvider.GetAlias(sequence.NonNullValue());
            eventEmitter.Emit(new SequenceStartEventInfo(sequence) { Anchor = anchor }, context);
        }

        public override void VisitScalar(IObjectDescriptor scalar, IEmitter context)
        {
            var scalarInfo = new ScalarEventInfo(scalar);
            if (scalar.Value != null)
            {
                scalarInfo.Anchor = aliasProvider.GetAlias(scalar.Value);
            }
            eventEmitter.Emit(scalarInfo, context);
        }
    }
}
