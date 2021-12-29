using System.Collections.Generic;
using System.Text;

namespace EasyExceptions.WritingRules
{
    public class ExcludeWatsonBucketsPropertiesWriter : IExcPartWriter
    {
        public void Apply(StringBuilder resultBuilder, object obj, Dictionary<string, object> propertiesToBeWritten)
        {
            propertiesToBeWritten.Remove("IPForWatsonBuckets");
            propertiesToBeWritten.Remove("WatsonBuckets");
        }
    }
}
