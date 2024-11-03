using System;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization.EventEmitters
{
    /// <summary>
    /// Provided the base implementation for an IEventEmitter that is a
    /// decorator for another IEventEmitter.
    /// </summary>
    public abstract class ChainedEventEmitter : IEventEmitter
    {
        protected readonly IEventEmitter nextEmitter;

        protected ChainedEventEmitter(IEventEmitter nextEmitter)
        {
            this.nextEmitter = nextEmitter ?? throw new ArgumentNullException(nameof(nextEmitter));
        }

        public virtual void Emit(AliasEventInfo eventInfo, IEmitter emitter)
        {
            nextEmitter.Emit(eventInfo, emitter);
        }

        public virtual void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
        {
            nextEmitter.Emit(eventInfo, emitter);
        }

        public virtual void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
        {
            nextEmitter.Emit(eventInfo, emitter);
        }

        public virtual void Emit(MappingEndEventInfo eventInfo, IEmitter emitter)
        {
            nextEmitter.Emit(eventInfo, emitter);
        }

        public virtual void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
        {
            nextEmitter.Emit(eventInfo, emitter);
        }

        public virtual void Emit(SequenceEndEventInfo eventInfo, IEmitter emitter)
        {
            nextEmitter.Emit(eventInfo, emitter);
        }
    }
}
