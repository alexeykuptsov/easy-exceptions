using System;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization
{
    /// <summary>
    /// Represents an object along with its type.
    /// </summary>
    public interface IObjectDescriptor
    {
        /// <summary>
        /// A reference to the object.
        /// </summary>
        object? Value { get; }

        /// <summary>
        /// The type that should be used when to interpret the <see cref="Value" />.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// The type of <see cref="Value" /> as determined by its container (e.g. a property).
        /// </summary>
        Type StaticType { get; }

        /// <summary>
        /// The style to be used for scalars.
        /// </summary>
        ScalarStyle ScalarStyle { get; }
    }

    public static class ObjectDescriptorExtensions
    {
        /// <summary>
        /// Returns the Value property of the <paramref name="objectDescriptor"/> if it is not null.
        /// This is useful in all places that the value must not be null.
        /// </summary>
        /// <param name="objectDescriptor">An object descriptor.</param>
        /// <exception cref="InvalidOperationException">Thrown when the Value is null</exception>
        /// <returns></returns>
        public static object NonNullValue(this IObjectDescriptor objectDescriptor)
        {
            return objectDescriptor.Value ?? throw new InvalidOperationException($"Attempted to use a IObjectDescriptor of type '{objectDescriptor.Type.FullName}' whose Value is null at a point whete it is invalid to do so. This may indicate a bug in YamlDotNet.");
        }
    }
}
