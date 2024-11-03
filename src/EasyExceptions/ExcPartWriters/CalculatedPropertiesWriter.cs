using System;
using System.Collections.Generic;
using System.Text;

namespace EasyExceptions.ExcPartWriters
{
    public class CalculatedPropertiesWriter : IExcPartWriter
    {
        public void Apply(StringBuilder resultBuilder, object obj, Dictionary<string, object> propertiesToBeWritten)
        {
            var exception = obj as Exception;
            if (exception == null) 
                return;

            resultBuilder.AppendFormat("@PathFromRootException: {0}",
                exception.Data[ExceptionDumpUtil.ServiceDataPrefix + " PathFromRootException"]);
            resultBuilder.AppendLine();

            resultBuilder.AppendFormat("@GetType().FullName: {0}", exception.GetType().FullName);
            resultBuilder.AppendLine();
        }
    }
}