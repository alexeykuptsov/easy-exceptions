using System.Text;

namespace EasyExceptions.NameValueWriters
{
    public interface IPropertyWriter
    {
        bool IsValueApplicable(object value);
        void Write(StringBuilder resultBuilder, string name, object value);
    }
}