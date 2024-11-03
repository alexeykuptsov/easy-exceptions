using System.Collections.Generic;
using System.Linq;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization.ObjectGraphVisitors
{
    public sealed class CustomSerializationObjectGraphVisitor : ChainedObjectGraphVisitor
    {
        private readonly IEnumerable<IYamlTypeConverter> typeConverters;
        private readonly ObjectSerializer nestedObjectSerializer;

        public CustomSerializationObjectGraphVisitor(IObjectGraphVisitor<IEmitter> nextVisitor, IEnumerable<IYamlTypeConverter> typeConverters, ObjectSerializer nestedObjectSerializer)
            : base(nextVisitor)
        {
            this.typeConverters = typeConverters.ToList();

            this.nestedObjectSerializer = nestedObjectSerializer;
        }

        public override bool Enter(IObjectDescriptor value, IEmitter context)
        {
            var typeConverter = typeConverters.FirstOrDefault(t => t.Accepts(value.Type));
            if (typeConverter != null)
            {
                typeConverter.WriteYaml(context, value.Value, value.Type);
                return false;
            }

            if (value.Value is IYamlConvertible convertible)
            {
                convertible.Write(context, nestedObjectSerializer);
                return false;
            }

            return base.Enter(value, context);
        }
    }
}
