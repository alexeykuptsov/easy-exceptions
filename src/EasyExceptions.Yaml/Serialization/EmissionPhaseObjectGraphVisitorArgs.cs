using System;
using System.Collections.Generic;
using System.Linq;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization
{
    public sealed class EmissionPhaseObjectGraphVisitorArgs
    {
        /// <summary>
        /// Gets the next visitor that should be called by the current visitor.
        /// </summary>
        public IObjectGraphVisitor<IEmitter> InnerVisitor { get; private set; }

        /// <summary>
        /// Gets the <see cref="IEventEmitter" /> that is to be used for serialization.
        /// </summary>
        public IEventEmitter EventEmitter { get; private set; }

        /// <summary>
        /// Gets a function that, when called, serializes the specified object.
        /// </summary>
        public ObjectSerializer NestedObjectSerializer { get; private set; }

        public IEnumerable<IYamlTypeConverter> TypeConverters { get; private set; }

        private readonly IEnumerable<IObjectGraphVisitor<Nothing>> preProcessingPhaseVisitors;

        public EmissionPhaseObjectGraphVisitorArgs(
            IObjectGraphVisitor<IEmitter> innerVisitor,
            IEventEmitter eventEmitter,
            IEnumerable<IObjectGraphVisitor<Nothing>> preProcessingPhaseVisitors,
            IEnumerable<IYamlTypeConverter> typeConverters,
            ObjectSerializer nestedObjectSerializer
        )
        {
            InnerVisitor = innerVisitor ?? throw new ArgumentNullException(nameof(innerVisitor));
            EventEmitter = eventEmitter ?? throw new ArgumentNullException(nameof(eventEmitter));
            this.preProcessingPhaseVisitors = preProcessingPhaseVisitors ?? throw new ArgumentNullException(nameof(preProcessingPhaseVisitors));
            TypeConverters = typeConverters ?? throw new ArgumentNullException(nameof(typeConverters));
            NestedObjectSerializer = nestedObjectSerializer ?? throw new ArgumentNullException(nameof(nestedObjectSerializer));
        }

        /// <summary>
        /// Gets the visitor of type <typeparamref name="T" /> that was used during the pre-processing phase.
        /// </summary>
        /// <typeparam name="T">The type of the visitor.s</typeparam>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException">
        /// No visitor of that type has been registered,
        /// or ore than one visitors registered are of type <typeparamref name="T"/>.
        /// </exception>
        public T GetPreProcessingPhaseObjectGraphVisitor<T>()
            where T : IObjectGraphVisitor<Nothing>
        {
            return preProcessingPhaseVisitors
                .OfType<T>()
                .Single();
        }
    }
}
