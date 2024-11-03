namespace EasyExceptions.Yaml.Core
{
    /// <summary>
    /// Exception that is thrown when an infinite recursion is detected.
    /// </summary>
    public sealed class MaximumRecursionLevelReachedException : YamlException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MaximumRecursionLevelReachedException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public MaximumRecursionLevelReachedException(string message)
            : base(message)
        {
        }
    }
}
