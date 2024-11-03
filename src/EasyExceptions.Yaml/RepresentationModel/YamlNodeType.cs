namespace EasyExceptions.Yaml.RepresentationModel
{
    /// <summary>
    /// Specifies the type of node in the representation model.
    /// </summary>
    public enum YamlNodeType
    {
        /// <summary>
        /// The node is a <see cref="YamlScalarNode"/>.
        /// </summary>
        Scalar,

        /// <summary>
        /// The node is a <see cref="YamlSequenceNode"/>.
        /// </summary>
        Sequence
    }
}
