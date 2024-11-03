namespace EasyExceptions.Yaml.Serialization
{
    public interface ISerializer
    {
        /// <summary>
        /// Serializes the specified object into a string.
        /// </summary>
        /// <param name="graph">The object to serialize.</param>
        string Serialize(object? graph);
    }
}
