using System;
using System.Collections.Generic;
using System.Globalization;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization.ObjectGraphVisitors
{
    public sealed class AnchorAssigner : PreProcessingPhaseObjectGraphVisitorSkeleton, IAliasProvider
    {
        private class AnchorAssignment
        {
            public AnchorName Anchor;
        }

        private readonly IDictionary<object, AnchorAssignment> assignments = new Dictionary<object, AnchorAssignment>();
        private uint nextId;

        public AnchorAssigner(IEnumerable<IYamlTypeConverter> typeConverters)
            : base(typeConverters)
        {
        }

        protected override bool Enter(IObjectDescriptor value)
        {
            if (value.Value != null && assignments.TryGetValue(value.Value, out var assignment))
            {
                if (assignment.Anchor.IsEmpty)
                {
                    assignment.Anchor = new AnchorName("o" + nextId.ToString(CultureInfo.InvariantCulture));
                    ++nextId;
                }
                return false;
            }

            return true;
        }

        protected override bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value)
        {
            return true;
        }

        protected override bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value)
        {
            return true;
        }

        protected override void VisitScalar(IObjectDescriptor scalar)
        {
            // Do not assign anchors to scalars
        }

        protected override void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType)
        {
            VisitObject(mapping);
        }

        protected override void VisitMappingEnd(IObjectDescriptor mapping) { }

        protected override void VisitSequenceStart(IObjectDescriptor sequence, Type elementType)
        {
            VisitObject(sequence);
        }

        protected override void VisitSequenceEnd(IObjectDescriptor sequence) { }

        private void VisitObject(IObjectDescriptor value)
        {
            if (value.Value != null)
            {
                assignments.Add(value.Value, new AnchorAssignment());
            }
        }

        AnchorName IAliasProvider.GetAlias(object target)
        {
            if (target != null && assignments.TryGetValue(target, out var assignment))
            {
                return assignment.Anchor;
            }
            return AnchorName.Empty;
        }
    }
}
