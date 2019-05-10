using System;
using System.Collections.Generic;
using System.Text;
using EasyExceptions.Utils;

namespace EasyExceptions.WritingRules
{
    public class MessageRule : IWritingRule
    {
        public void Apply(StringBuilder resultBuilder, object obj, Dictionary<string, object> allProperties)
        {
            var exception = obj as Exception;
            if (exception == null)
                return;

            resultBuilder.AppendFormat("{0} = {1}", "Message",
                exception.Message.QuoteIfContainsWhitespaces());
            resultBuilder.AppendLine();

            allProperties.Remove("Message");
        }
    }
}