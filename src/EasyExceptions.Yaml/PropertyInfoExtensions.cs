using System.Reflection;

namespace EasyExceptions.Yaml
{
    internal static class PropertyInfoExtensions
    {
        public static object? ReadValue(this PropertyInfo property, object target)
        {
            return property.GetValue(target, null);
        }
    }
}
