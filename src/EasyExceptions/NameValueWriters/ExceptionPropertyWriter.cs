using System;
using System.Text;

namespace EasyExceptions.NameValueWriters
{
    class ExceptionPropertyWriter : PropertyWriterBase<Exception>
    {
        protected override void WriteInternal(StringBuilder resultBuilder, string name, Exception dictionary)
        {
            resultBuilder.AppendFormat("{0} = {1}", name,
                dictionary.Data[ExceptionDumpUtil.ServiceDataPrefix + " PathFromRootException"]);
            resultBuilder.AppendLine();
        }
    }
}