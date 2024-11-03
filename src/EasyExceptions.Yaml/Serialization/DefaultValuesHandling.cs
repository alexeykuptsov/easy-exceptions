using System;

namespace EasyExceptions.Yaml.Serialization
{
    /// <summary>
    /// Specifies the strategy to handle default and null values during serialization of properties.
    /// </summary>
    [Flags]
    public enum DefaultValuesHandling
    {
        /// <summary>
        /// Specifies that all properties are to be emitted regardless of their value. This is the default behavior.
        /// </summary>
        Preserve = 0,

        /// <summary>
        /// Specifies that properties that contain null references or a null Nullable&lt;T&gt; are to be omitted.
        /// </summary>
        OmitNull = 1,

        /// <summary>
        /// Specifies that properties that that contain their default value, either default(T) or the value specified in DefaultValueAttribute are to be omitted.
        /// </summary>
        OmitDefaults = 2,

        /// <summary>
        /// Specifies that properties that that contain collections/arrays/enumerations that are empty are to be omitted.
        /// </summary>
        OmitEmptyCollections = 4,
    }
}
