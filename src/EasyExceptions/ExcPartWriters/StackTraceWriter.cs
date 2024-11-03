using System;
using System.Collections.Generic;
using System.Text;

namespace EasyExceptions.ExcPartWriters
{
    public class StackTraceWriter : IExcPartWriter
    {
        public void Apply(StringBuilder resultBuilder, object obj, Dictionary<string, object> propertiesToBeWritten)
        {
            var exception = obj as Exception;
            if (exception == null)
                return;

            resultBuilder.AppendFormat("{0}: ``", "StackTrace").AppendLine();
            resultBuilder.Append(exception.StackTrace).AppendLine();
            resultBuilder.Append("``").AppendLine();

            propertiesToBeWritten.Remove("StackTrace");
        }
    }
}