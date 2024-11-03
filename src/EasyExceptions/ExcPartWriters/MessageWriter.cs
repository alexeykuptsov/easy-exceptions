using System;
using System.Collections.Generic;
using System.Text;

namespace EasyExceptions.ExcPartWriters
{
    public class MessageWriter : IExcPartWriter
    {
        public void Apply(StringBuilder resultBuilder, object obj, Dictionary<string, object> propertiesToBeWritten)
        {
            var exception = obj as Exception;
            if (exception == null)
                return;

            resultBuilder.AppendFormat("{0}: {1}", "Message", exception.Message);
            resultBuilder.AppendLine();

            propertiesToBeWritten.Remove("Message");
        }
    }
}