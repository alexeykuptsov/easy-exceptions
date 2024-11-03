using System;

namespace EasyExceptions.Yaml.Serialization
{
    /// <summary>
    /// Defined the interface of a type that can be notified during an object graph traversal.
    /// </summary>
    public interface IObjectGraphVisitor<TContext>
    {
        /// <summary>
        /// Indicates whether the specified value should be entered. This allows the visitor to
        /// override the handling of a particular object or type.
        /// </summary>
        /// <param name="value">The value that is about to be entered.</param>
        /// <param name="context">The context that this implementation depend on.</param>
        /// <returns>If the value is to be entered, returns true; otherwise returns false;</returns>
        bool Enter(IObjectDescriptor value, TContext context);

        /// <summary>
        /// Indicates whether the specified mapping should be entered. This allows the visitor to
        /// override the handling of a particular pair.
        /// </summary>
        /// <param name="key">The key of the mapping that is about to be entered.</param>
        /// <param name="value">The value of the mapping that is about to be entered.</param>
        /// <param name="context">The context that this implementation depend on.</param>
        /// <returns>If the mapping is to be entered, returns true; otherwise returns false;</returns>
        bool EnterMapping(IObjectDescriptor key, IObjectDescriptor value, TContext context);

        /// <summary>
        /// Indicates whether the specified mapping should be entered. This allows the visitor to
        /// override the handling of a particular pair. This overload should be invoked when the
        /// mapping is produced by an object's property.
        /// </summary>
        /// <param name="key">The <see cref="IPropertyDescriptor"/> that provided access to <paramref name="value"/>.</param>
        /// <param name="value">The value of the mapping that is about to be entered.</param>
        /// <param name="context">The context that this implementation depend on.</param>
        /// <returns>If the mapping is to be entered, returns true; otherwise returns false;</returns>
        bool EnterMapping(IPropertyDescriptor key, IObjectDescriptor value, TContext context);

        /// <summary>
        /// Notifies the visitor that a scalar value has been encountered.
        /// </summary>
        /// <param name="scalar">The value of the scalar.</param>
        /// <param name="context">The context that this implementation depend on.</param>
        void VisitScalar(IObjectDescriptor scalar, TContext context);

        /// <summary>
        /// Notifies the visitor that the traversal of a mapping is about to begin.
        /// </summary>
        /// <param name="mapping">The value that corresponds to the mapping.</param>
        /// <param name="keyType">The static type of the keys of the mapping.</param>
        /// <param name="valueType">The static type of the values of the mapping.</param>
        /// <param name="context">The context that this implementation depend on.</param>
        void VisitMappingStart(IObjectDescriptor mapping, Type keyType, Type valueType, TContext context);

        /// <summary>
        /// Notifies the visitor that the traversal of a mapping has ended.
        /// </summary>
        /// <param name="mapping">The value that corresponds to the mapping.</param>
        /// <param name="context">The context that this implementation depend on.</param>
        void VisitMappingEnd(IObjectDescriptor mapping, TContext context);

        /// <summary>
        /// Notifies the visitor that the traversal of a sequence is about to begin.
        /// </summary>
        /// <param name="sequence">The value that corresponds to the sequence.</param>
        /// <param name="elementType">The static type of the elements of the sequence.</param>
        /// <param name="context">The context that this implementation depend on.</param>
        void VisitSequenceStart(IObjectDescriptor sequence, Type elementType, TContext context);

        /// <summary>
        /// Notifies the visitor that the traversal of a sequence has ended.
        /// </summary>
        /// <param name="sequence">The value that corresponds to the sequence.</param>
        /// <param name="context">The context that this implementation depend on.</param>
        void VisitSequenceEnd(IObjectDescriptor sequence, TContext context);
    }
}
