using System.Text;

namespace EasyExceptions.NameValueWriters
{
    public interface INameValueWriter
    {
        bool IsValueApplicable(object value);
        void Write(StringBuilder resultBuilder, string name, object value);
    }
}