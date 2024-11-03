using EasyExceptions.Yaml.Core;

namespace EasyExceptions.Yaml.Serialization
{
    public interface IAliasProvider
    {
        AnchorName GetAlias(object target);
    }
}
