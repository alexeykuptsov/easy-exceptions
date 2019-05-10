using System;
using System.Text;
using EasyExceptions.Utils;

namespace EasyExceptions.NameValueWriters
{
    public class SimplePropertyWriter : PropertyWriterBase<object>
    {
        protected override void WriteInternal(StringBuilder resultBuilder, string name, object value)
        {
            resultBuilder.AppendFormat("{0} = {1}", name,
                Convert.ToString(value).QuoteIfContainsWhitespaces());
            resultBuilder.AppendLine();
        }
    }
}