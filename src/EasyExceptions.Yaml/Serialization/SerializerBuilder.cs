using System;
using System.Collections.Generic;
using EasyExceptions.Yaml.Core;
using EasyExceptions.Yaml.Serialization.EventEmitters;
using EasyExceptions.Yaml.Serialization.ObjectFactories;
using EasyExceptions.Yaml.Serialization.ObjectGraphTraversalStrategies;
using EasyExceptions.Yaml.Serialization.ObjectGraphVisitors;
using EasyExceptions.Yaml.Serialization.TypeInspectors;
using EasyExceptions.Yaml.Serialization.TypeResolvers;
#if NET7_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
#endif

namespace EasyExceptions.Yaml.Serialization
{

    /// <summary>
    /// Creates and configures instances of <see cref="Serializer" />.
    /// This class is used to customize the behavior of <see cref="Serializer" />. Use the relevant methods
    /// to apply customizations, then call <see cref="Build" /> to create an instance of the serializer
    /// with the desired customizations.
    /// </summary>
#if NET7_0_OR_GREATER
    [RequiresDynamicCode("This builder configures the serializer to use reflection which is not compatible with ahead-of-time compilation or assembly trimming." +
        " You need to use the code generator/analyzer to generate static code and use the 'StaticSerializerBuilder' object instead of this one.")]
#endif
    public sealed class SerializerBuilder : BuilderSkeleton<SerializerBuilder>
    {
        private readonly ObjectGraphTraversalStrategyFactory objectGraphTraversalStrategyFactory;
        private readonly LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories;
        private readonly LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories;
        private readonly LazyComponentRegistrationList<IEventEmitter, IEventEmitter> eventEmitterFactories;
        private readonly IDictionary<Type, TagName> tagMappings = new Dictionary<Type, TagName>();
        private readonly IObjectFactory objectFactory;
        private readonly int maximumRecursion = 50;
        private readonly EmitterSettings emitterSettings = EmitterSettings.Default;
        private readonly DefaultValuesHandling defaultValuesHandlingConfiguration = DefaultValuesHandling.Preserve;
        private readonly ScalarStyle defaultScalarStyle = ScalarStyle.Any;
        private Func<Exception, object, string, string>? exceptionHandler;

        public SerializerBuilder()
            : base(new DynamicTypeResolver())
        {
            typeInspectorFactories.Add(inner => new CachedTypeInspector(inner));
            typeInspectorFactories.Add(inner => inner);
            typeInspectorFactories.Add(inner => new YamlAttributesTypeInspector(inner));
            typeInspectorFactories.Add(inner => new YamlAttributeOverridesInspector(inner, overrides.Clone()));

            preProcessingPhaseObjectGraphVisitorFactories = new LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>>
            {
                typeConverters => new AnchorAssigner(typeConverters)
            };

            emissionPhaseObjectGraphVisitorFactories = new LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>>
            {
                args => new CustomSerializationObjectGraphVisitor(args.InnerVisitor, args.TypeConverters, args.NestedObjectSerializer),
                args => new AnchorAssigningObjectGraphVisitor(args.InnerVisitor, args.EventEmitter, args.GetPreProcessingPhaseObjectGraphVisitor<AnchorAssigner>()),
                args => new DefaultValuesObjectGraphVisitor(defaultValuesHandlingConfiguration, args.InnerVisitor, new DefaultObjectFactory()),
                args => new CommentsObjectGraphVisitor(args.InnerVisitor)
            };

            eventEmitterFactories = new LazyComponentRegistrationList<IEventEmitter, IEventEmitter>
            {
                inner =>
                        new TypeAssigningEventEmitter(inner,
                            tagMappings,
                            defaultScalarStyle,
                            yamlFormatter)
            };

            objectFactory = new DefaultObjectFactory();

            objectGraphTraversalStrategyFactory = (typeInspector, typeResolver, typeConverters, maximumRecursion) =>
                new FullObjectGraphTraversalStrategy(typeInspector, typeResolver, maximumRecursion, objectFactory);
        }

        /// <summary>
        /// Enables handling an exception thrown by a property so that information about exception is serialized as string value of the property.
        /// </summary>
        /// <param name="exceptionHandler">
        /// A function that takes the caught exception, the object and the name of the object's property (from which the exception was thrown).
        /// The string returned from the function is written instead of the property value by the serializer.
        /// </param>
        public SerializerBuilder WithExceptionHandler(Func<Exception, object, string, string> exceptionHandler)
        {
            this.exceptionHandler = exceptionHandler;
            return this;
        }


        /// <summary>
        /// Creates a new <see cref="Serializer" /> according to the current configuration.
        /// </summary>
        public ISerializer Build()
        {
            return Serializer.FromValueSerializer(BuildValueSerializer(), emitterSettings);
        }

        /// <summary>
        /// Creates a new <see cref="IValueSerializer" /> that implements the current configuration.
        /// This method is available for advanced scenarios. The preferred way to customize the behavior of the
        /// serializer is to use the <see cref="Build" /> method.
        /// </summary>
        public IValueSerializer BuildValueSerializer()
        {
            var typeConverters = BuildTypeConverters();
            var typeInspector = BuildTypeInspector();
            var traversalStrategy = objectGraphTraversalStrategyFactory(typeInspector, typeResolver, typeConverters, maximumRecursion);
            var eventEmitter = eventEmitterFactories.BuildComponentChain(new WriterEventEmitter());

            return new ValueSerializer(
                traversalStrategy,
                eventEmitter,
                typeConverters,
                preProcessingPhaseObjectGraphVisitorFactories.Clone(),
                emissionPhaseObjectGraphVisitorFactories.Clone()
            );
        }

        internal ITypeInspector BuildTypeInspector()
        {
            ITypeInspector innerInspector = new ReadablePropertiesTypeInspector(
                typeResolver, includeNonPublicProperties, exceptionHandler);

            innerInspector = new CompositeTypeInspector(new ReadableFieldsTypeInspector(typeResolver), innerInspector);

            return typeInspectorFactories.BuildComponentChain(innerInspector);
        }

        private class ValueSerializer : IValueSerializer
        {
            private readonly IObjectGraphTraversalStrategy traversalStrategy;
            private readonly IEventEmitter eventEmitter;
            private readonly IEnumerable<IYamlTypeConverter> typeConverters;
            private readonly LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories;
            private readonly LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories;

            public ValueSerializer(
                IObjectGraphTraversalStrategy traversalStrategy,
                IEventEmitter eventEmitter,
                IEnumerable<IYamlTypeConverter> typeConverters,
                LazyComponentRegistrationList<IEnumerable<IYamlTypeConverter>, IObjectGraphVisitor<Nothing>> preProcessingPhaseObjectGraphVisitorFactories,
                LazyComponentRegistrationList<EmissionPhaseObjectGraphVisitorArgs, IObjectGraphVisitor<IEmitter>> emissionPhaseObjectGraphVisitorFactories
            )
            {
                this.traversalStrategy = traversalStrategy;
                this.eventEmitter = eventEmitter;
                this.typeConverters = typeConverters;
                this.preProcessingPhaseObjectGraphVisitorFactories = preProcessingPhaseObjectGraphVisitorFactories;
                this.emissionPhaseObjectGraphVisitorFactories = emissionPhaseObjectGraphVisitorFactories;
            }

            public void SerializeValue(IEmitter emitter, object? value, Type? type)
            {
                var actualType = type ?? (value != null ? value.GetType() : typeof(object));
                var staticType = type ?? typeof(object);

                var graph = new ObjectDescriptor(value, actualType, staticType);

                var preProcessingPhaseObjectGraphVisitors = preProcessingPhaseObjectGraphVisitorFactories.BuildComponentList(typeConverters);
                foreach (var visitor in preProcessingPhaseObjectGraphVisitors)
                {
                    traversalStrategy.Traverse(graph, visitor, default);
                }

                void NestedObjectSerializer(object? v, Type? t) => SerializeValue(emitter, v, t);

                var emittingVisitor = emissionPhaseObjectGraphVisitorFactories.BuildComponentChain(
                    new EmittingObjectGraphVisitor(eventEmitter),
                    inner => new EmissionPhaseObjectGraphVisitorArgs(inner, eventEmitter, preProcessingPhaseObjectGraphVisitors, typeConverters, NestedObjectSerializer)
                );

                traversalStrategy.Traverse(graph, emittingVisitor, emitter);
            }
        }
    }
}
