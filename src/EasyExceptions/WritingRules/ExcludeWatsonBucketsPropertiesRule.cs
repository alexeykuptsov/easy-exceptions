using System;
using System.Collections.Generic;
using System.Text;

namespace EasyExceptions.WritingRules
{
    public class ExcludeWatsonBucketsPropertiesRule : IWritingRule
    {
        public void Apply(StringBuilder resultBuilder, object obj, Dictionary<string, object> allProperties)
        {
            allProperties.Remove("IPForWatsonBuckets");
            allProperties.Remove("WatsonBuckets");
        }
    }
}
