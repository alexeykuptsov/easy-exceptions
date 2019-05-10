using System.Reflection;
using System.Text;
using EasyExceptions.WritingRules;

namespace EasyExceptions.NameValueWriters
{
    public abstract class ReflectionPropertyWriterBase : IPropertyWriter
    {
        public bool IsValueApplicable(object value)
        {
            return value != null && value.GetType().FullName == GetFullTypeName();
        }

        protected abstract string GetFullTypeName();

        public abstract void Write(StringBuilder resultBuilder, string name, object value);

        protected static void WritePropertyValue(StringBuilder resultBuilder, string ownerName, object ownerValue, string propertyName)
        {
            var propertyInfo = TryGetSingleReadablePropertyWithoutParameters(ownerValue, propertyName);
            if (propertyInfo == null)
                return;
            var propertyValue = propertyInfo.GetValue(ownerValue, new object[0]);
            RegularPropertiesRule.WriteNameValue(resultBuilder, ownerName + "." + propertyName, propertyValue);
        }

        protected static PropertyInfo TryGetSingleReadablePropertyWithoutParameters(object value, string propertyName)
        {
            PropertyInfo property;
            try
            {
                property = value.GetType().GetRuntimeProperty(propertyName);
            }
            catch (AmbiguousMatchException)
            {
                return null;
            }
            if (property == null || !property.CanRead || property.GetMethod.GetParameters().Length != 0)
                return null;
            return property;
        }
    }
}