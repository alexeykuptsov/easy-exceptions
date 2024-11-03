using System;
using System.Collections.Generic;
using EasyExceptions.Yaml.Core;
using EasyExceptions.Yaml.Serialization.Schemas;

namespace EasyExceptions.Yaml.Serialization.EventEmitters
{
    public sealed class TypeAssigningEventEmitter : ChainedEventEmitter
    {
        private readonly IDictionary<Type, TagName> tagMappings;
        private readonly ScalarStyle defaultScalarStyle;
        private readonly YamlFormatter formatter;

        public TypeAssigningEventEmitter(IEventEmitter nextEmitter,
            IDictionary<Type, TagName> tagMappings,
            ScalarStyle defaultScalarStyle,
            YamlFormatter formatter)
            : base(nextEmitter)
        {
            this.defaultScalarStyle = defaultScalarStyle;
            this.formatter = formatter;
            this.tagMappings = tagMappings;
        }

        public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
        {
            var suggestedStyle = ScalarStyle.Plain;

            var value = eventInfo.Source.Value;
            if (value == null)
            {
                eventInfo.Tag = JsonSchema.Tags.Null;
                eventInfo.RenderedValue = "";
            }
            else
            {
                var typeCode = eventInfo.Source.Type.GetTypeCode();
                switch (typeCode)
                {
                    case TypeCode.Boolean:
                        eventInfo.Tag = JsonSchema.Tags.Bool;
                        eventInfo.RenderedValue = formatter.FormatBoolean(value);
                        break;

                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.Int32:
                    case TypeCode.Int64:
                    case TypeCode.SByte:
                    case TypeCode.UInt16:
                    case TypeCode.UInt32:
                    case TypeCode.UInt64:
                        //Enum's are special cases, they fall in here, but get sent out as a string.
                        if (eventInfo.Source.Type.IsEnum)
                        {
                            eventInfo.Tag = FailsafeSchema.Tags.Str;
                            eventInfo.RenderedValue = formatter.FormatEnum(value);

                            suggestedStyle = defaultScalarStyle;
                        }
                        else
                        {
                            eventInfo.Tag = JsonSchema.Tags.Int;
                            eventInfo.RenderedValue = formatter.FormatNumber(value);
                        }
                        break;

                    case TypeCode.Single:
                        eventInfo.Tag = JsonSchema.Tags.Float;
                        eventInfo.RenderedValue = formatter.FormatNumber((float)value);
                        break;

                    case TypeCode.Double:
                        eventInfo.Tag = JsonSchema.Tags.Float;
                        eventInfo.RenderedValue = formatter.FormatNumber((double)value);
                        break;

                    case TypeCode.Decimal:
                        eventInfo.Tag = JsonSchema.Tags.Float;
                        eventInfo.RenderedValue = formatter.FormatNumber(value);
                        break;

                    case TypeCode.String:
                    case TypeCode.Char:
                        eventInfo.Tag = FailsafeSchema.Tags.Str;
                        eventInfo.RenderedValue = value.ToString()!;

                        suggestedStyle = defaultScalarStyle;

                        break;

                    case TypeCode.DateTime:
                        eventInfo.Tag = DefaultSchema.Tags.Timestamp;
                        eventInfo.RenderedValue = formatter.FormatDateTime(value);
                        break;

                    case TypeCode.Empty:
                        eventInfo.Tag = JsonSchema.Tags.Null;
                        eventInfo.RenderedValue = "";
                        break;

                    default:
                        if (eventInfo.Source.Type == typeof(TimeSpan))
                        {
                            eventInfo.RenderedValue = formatter.FormatTimeSpan(value);
                            break;
                        }

                        throw new NotSupportedException($"TypeCode.{typeCode} is not supported.");
                }
            }

            eventInfo.IsPlainImplicit = true;
            if (eventInfo.Style == ScalarStyle.Any)
            {
                eventInfo.Style = suggestedStyle;
            }

            base.Emit(eventInfo, emitter);
        }

        public override void Emit(MappingStartEventInfo eventInfo, IEmitter emitter)
        {
            AssignTypeIfNeeded(eventInfo);
            base.Emit(eventInfo, emitter);
        }

        public override void Emit(SequenceStartEventInfo eventInfo, IEmitter emitter)
        {
            AssignTypeIfNeeded(eventInfo);
            base.Emit(eventInfo, emitter);
        }

        private void AssignTypeIfNeeded(ObjectEventInfo eventInfo)
        {
            if (tagMappings.TryGetValue(eventInfo.Source.Type, out var tag))
            {
                eventInfo.Tag = tag;
            }
        }
    }
}
