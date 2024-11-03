using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyExceptions.Yaml.Serialization.ObjectGraphVisitors
{
    /// <summary>
    /// A base class that simplifies the correct implementation of <see cref="IObjectGraphVisitor{Nothing}" />.
    /// </summary>
    public abstract class PreProcessingPhaseObjectGraphVisitorSkeleton : IObjectGraphVisitor<Nothing>
    {
        protected readonly IEnumerable<IYamlTypeConverter> typeConverters;

        public PreProcessingPhaseObjectGraphVisitorSkeleton(IEnumerable<IYamlTypeConverter> typeConverters)
        {
            this.typeConverters = typeConverters.ToList();
        }

        bool IObjectGraphVisitor<Nothing>.Enter(IObjectDescriptor value, Nothing context)
        {
            var typeConverter = typeConverters.FirstOrDefault(t => t.Accepts(value.Type));
            if (typeConverter != null)
            {
                return false;
            }

            if (value.Value is IYamlConvertible convertible)
            {
                return false;
            }

            return Enter(value);
        }

        bool IObjectGraphVisitor<Nothing>.EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, Nothing context)
        {
            return EnterMapping(key, value);
        }

        bool IObjectGraphVisitor<Nothing>.EnterMapping(IObjectDescriptor key, IObjectDescriptor value, Nothing context)
        {
            return EnterMapping(key, value);
        }

        void IObjectGraphVisitor<Nothing>.VisitMappingEnd(IObjectDescriptor mapping, Nothing context)
        {
            VisitMappingEnd(mapping);
        }

        void IObjectGraphVisitor<Nothing>.VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, Nothing context)
        {
            VisitMappingStart(mapping, keyType, valueType);
        }

        void IObjectGraphVisitor<Nothing>.VisitScalar(IObjectDescriptor scalar, Nothing context)
        {
            VisitScalar(scalar);
        }

        void IObjectGraphVisitor<Nothing>.VisitSequenceEnd(IObjectDescriptor sequence, Nothing context)
        {
            VisitSequenceEnd(sequence);
        }

        void IObjectGraphVisitor<Nothing>.VisitSequenceStart(IObjectDescriptor sequence, Type elementType, Nothing context)
        {
            VisitSequenceStart(sequence, elementType);
        }

        protected abstract bool Enter(IObjectDescriptor value);
        protected abstract bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value);
        protected abstract bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value);
        protected abstract void VisitMappingEnd(IObjectDescriptor mapping);
        protected abstract void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType);
        protected abstract void VisitScalar(IObjectDescriptor scalar);
        protected abstract void VisitSequenceEnd(IObjectDescriptor sequence);
        protected abstract void VisitSequenceStart(IObjectDescriptor sequence, Type elementType);
    }
}
