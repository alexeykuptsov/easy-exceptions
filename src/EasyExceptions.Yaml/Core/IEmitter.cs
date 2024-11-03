using EasyExceptions.Yaml.Core.Events;

namespace EasyExceptions.Yaml.Core
{
    /// <summary>
    /// Represents a YAML stream emitter.
    /// </summary>
    public interface IEmitter
    {
        /// <summary>
        /// Emits an event.
        /// </summary>
        void Emit(ParsingEvent @event);
    }
}
