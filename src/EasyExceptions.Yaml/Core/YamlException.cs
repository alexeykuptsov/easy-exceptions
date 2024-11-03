using System;

namespace EasyExceptions.Yaml.Core
{
    /// <summary>
    /// Base exception that is thrown when the a problem occurs in the YamlDotNet library.
    /// </summary>
    public class YamlException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YamlException"/> class.
        /// </summary>
        public YamlException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }

        public override string ToString()
        {
            return $"{Message}";
        }
    }
}
