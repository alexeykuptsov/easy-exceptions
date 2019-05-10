using System;
using System.Collections.Generic;
using System.Text;

namespace EasyExceptions.WritingRules
{
    public class ExcludeStackTraceRelatedPropertiesRule : IWritingRule
    {
        public void Apply(StringBuilder resultBuilder, object obj, Dictionary<string, object> allProperties)
        {
            allProperties.Remove("StackTrace");
            allProperties.Remove("TargetSite");
            allProperties.Remove("Source");
        }
    }
}
