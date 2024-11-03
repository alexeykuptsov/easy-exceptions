using System;
using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization
{
    public abstract class EventInfo
    {
        public IObjectDescriptor Source { get; }

        protected EventInfo(IObjectDescriptor source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
        }
    }

    public class AliasEventInfo : EventInfo
    {
        public AliasEventInfo(IObjectDescriptor source, AnchorName alias)
            : base(source)
        {
            if (alias.IsEmpty)
            {
                throw new ArgumentNullException(nameof(alias));
            }
            Alias = alias;
        }

        public AnchorName Alias { get; }
    }

    public class ObjectEventInfo : EventInfo
    {
        protected ObjectEventInfo(IObjectDescriptor source)
            : base(source)
        {
        }

        public AnchorName Anchor { get; set; }
        public TagName Tag { get; set; }
    }

    public sealed class ScalarEventInfo : ObjectEventInfo
    {
        public ScalarEventInfo(IObjectDescriptor source)
            : base(source)
        {
            Style = source.ScalarStyle;
            RenderedValue = string.Empty;
        }

        public string RenderedValue { get; set; }
        public ScalarStyle Style { get; set; }
        public bool IsPlainImplicit { get; set; }
    }

    public sealed class MappingStartEventInfo : ObjectEventInfo
    {
        public MappingStartEventInfo(IObjectDescriptor source)
            : base(source)
        {
        }
    }

    public sealed class MappingEndEventInfo : EventInfo
    {
        public MappingEndEventInfo(IObjectDescriptor source)
            : base(source)
        {
        }
    }

    public sealed class SequenceStartEventInfo : ObjectEventInfo
    {
        public SequenceStartEventInfo(IObjectDescriptor source)
            : base(source)
        {
        }

        public bool IsImplicit { get; set; }
    }

    public sealed class SequenceEndEventInfo : EventInfo
    {
        public SequenceEndEventInfo(IObjectDescriptor source)
            : base(source)
        {
        }
    }
}
