using System.Text;

namespace EasyExceptions.NameValueWriters
{
    public class DbValidationErrorPropertyWriter : ReflectionPropertyWriterBase
    {
        protected override string GetFullTypeName()
        {
            return "System.Data.Entity.Validation.DbValidationError";
        }

        public override void Write(StringBuilder resultBuilder, string name, object value)
        {
            WritePropertyValue(resultBuilder, name, value, "PropertyName");
            WritePropertyValue(resultBuilder, name, value, "ErrorMessage");
        }
    }
}