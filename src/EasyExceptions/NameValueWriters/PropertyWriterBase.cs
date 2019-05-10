using System.Text;

namespace EasyExceptions.NameValueWriters
{
    public abstract class PropertyWriterBase<T> : IPropertyWriter
    {
        public virtual bool IsValueApplicable(object value)
        {
            return value is T;
        }

        public void Write(StringBuilder resultBuilder, string name, object value)
        {
            WriteInternal(resultBuilder, name, (T)value);
        }

        protected abstract void WriteInternal(StringBuilder resultBuilder, string name, T dictionary);
    }
}