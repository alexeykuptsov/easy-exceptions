namespace EasyExceptions.Yaml.Core.Events
{
    internal enum EventType
    {
        StreamStart,
        StreamEnd,
        DocumentStart,
        DocumentEnd,
        Alias,
        Scalar,
        SequenceStart,
        SequenceEnd,
        MappingStart,
        MappingEnd,
    }
}
