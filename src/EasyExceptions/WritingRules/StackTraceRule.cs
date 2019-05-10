using System;
using System.Collections.Generic;
using System.Text;

namespace EasyExceptions.WritingRules
{
    public class StackTraceRule : IWritingRule
    {
        public void Apply(StringBuilder resultBuilder, object obj, Dictionary<string, object> allProperties)
        {
            var exception = obj as Exception;
            if (exception == null)
                return;

            resultBuilder.AppendFormat("{0} = ``", "StackTrace").AppendLine();
            resultBuilder.Append(exception.StackTrace).AppendLine();
            resultBuilder.Append("``").AppendLine();

            allProperties.Remove("StackTrace");
        }
    }
}