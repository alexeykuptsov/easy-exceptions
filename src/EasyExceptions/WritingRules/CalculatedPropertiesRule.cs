using System;
using System.Collections.Generic;
using System.Text;

namespace EasyExceptions.WritingRules
{
    public class CalculatedPropertiesRule : IWritingRule
    {
        public void Apply(StringBuilder resultBuilder, object obj, Dictionary<string, object> allProperties)
        {
            var exception = obj as Exception;
            if (exception == null) 
                return;

            resultBuilder.AppendFormat("@PathFromRootException = {0}",
                exception.Data[ExceptionDumpUtil.ServiceDataPrefix + " PathFromRootException"]);
            resultBuilder.AppendLine();

            resultBuilder.AppendFormat("@GetType().FullName = {0}", exception.GetType().FullName);
            resultBuilder.AppendLine();
        }
    }
}