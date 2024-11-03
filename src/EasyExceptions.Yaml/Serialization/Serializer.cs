using System;
using System.IO;
using EasyExceptions.Yaml.Core;
using EasyExceptions.Yaml.Core.Events;

namespace EasyExceptions.Yaml.Serialization
{
    public sealed class Serializer : ISerializer
    {
        private readonly IValueSerializer valueSerializer;
        private readonly EmitterSettings emitterSettings;

        /// <remarks>
        /// This constructor is private to discourage its use.
        /// To invoke it, call the <see cref="FromValueSerializer"/> method.
        /// </remarks>
        private Serializer(IValueSerializer valueSerializer, EmitterSettings emitterSettings)
        {
            this.valueSerializer = valueSerializer ?? throw new ArgumentNullException(nameof(valueSerializer));
            this.emitterSettings = emitterSettings ?? throw new ArgumentNullException(nameof(emitterSettings));
        }

        /// <summary>
        /// Creates a new <see cref="Serializer" /> that uses the specified <see cref="IValueSerializer" />.
        /// This method is available for advanced scenarios. The preferred way to customize the behavior of the
        /// serializer is to use <see cref="SerializerBuilder" />.
        /// </summary>
        public static Serializer FromValueSerializer(IValueSerializer valueSerializer, EmitterSettings emitterSettings)
        {
            return new Serializer(valueSerializer, emitterSettings);
        }

        /// <summary>
        /// Serializes the specified object into a string.
        /// </summary>
        /// <param name="graph">The object to serialize.</param>
        public string Serialize(object? graph)
        {
            using var buffer = new StringWriter();
            Serialize(buffer, graph);
            return buffer.ToString();
        }

        /// <summary>
        /// Serializes the specified object into a string.
        /// </summary>
        /// <param name="graph">The object to serialize.</param>
        /// <param name="type">The static type of the object to serialize.</param> 
        public string Serialize(object? graph, Type type)
        {
            using var buffer = new StringWriter();
            Serialize(buffer, graph, type);
            return buffer.ToString();
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter" /> where to serialize the object.</param>
        /// <param name="graph">The object to serialize.</param>
        public void Serialize(TextWriter writer, object? graph)
        {
            Serialize(new Emitter(writer, emitterSettings), graph);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="writer">The <see cref="TextWriter" /> where to serialize the object.</param>
        /// <param name="graph">The object to serialize.</param>
        /// <param name="type">The static type of the object to serialize.</param>
        public void Serialize(TextWriter writer, object? graph, Type type)
        {
            Serialize(new Emitter(writer, emitterSettings), graph, type);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="emitter">The <see cref="IEmitter" /> where to serialize the object.</param>
        /// <param name="graph">The object to serialize.</param>
        public void Serialize(IEmitter emitter, object? graph)
        {
            if (emitter == null)
            {
                throw new ArgumentNullException(nameof(emitter));
            }

            EmitDocument(emitter, graph, null);
        }

        /// <summary>
        /// Serializes the specified object.
        /// </summary>
        /// <param name="emitter">The <see cref="IEmitter" /> where to serialize the object.</param>
        /// <param name="graph">The object to serialize.</param>
        /// <param name="type">The static type of the object to serialize.</param>
        public void Serialize(IEmitter emitter, object? graph, Type type)
        {
            if (emitter == null)
            {
                throw new ArgumentNullException(nameof(emitter));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            EmitDocument(emitter, graph, type);
        }

        private void EmitDocument(IEmitter emitter, object? graph, Type? type)
        {
            emitter.Emit(new StreamStart());
            emitter.Emit(new DocumentStart());

            valueSerializer.SerializeValue(emitter, graph, type);

            emitter.Emit(new DocumentEnd(true));
            emitter.Emit(new StreamEnd());
        }
    }
}
