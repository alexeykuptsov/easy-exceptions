using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization.Schemas
{
    public static class FailsafeSchema
    {
        public static class Tags
        {
            public static readonly TagName Str = new TagName("tag:yaml.org,2002:str");
        }
    }

    public static class JsonSchema
    {
        public static class Tags
        {
            public static readonly TagName Null = new TagName("tag:yaml.org,2002:null");
            public static readonly TagName Bool = new TagName("tag:yaml.org,2002:bool");
            public static readonly TagName Int = new TagName("tag:yaml.org,2002:int");
            public static readonly TagName Float = new TagName("tag:yaml.org,2002:float");
        }
    }

    public static class DefaultSchema
    {
        public static class Tags
        {
            public static readonly TagName Timestamp = new TagName("tag:yaml.org,2002:timestamp");
        }
    }
}
