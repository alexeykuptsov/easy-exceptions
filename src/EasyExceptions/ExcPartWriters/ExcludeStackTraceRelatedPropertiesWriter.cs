using System.Collections.Generic;
using System.Text;

namespace EasyExceptions.ExcPartWriters
{
    public class ExcludeStackTraceRelatedPropertiesWriter : IExcPartWriter
    {
        public void Apply(StringBuilder resultBuilder, object obj, Dictionary<string, object> propertiesToBeWritten)
        {
            propertiesToBeWritten.Remove("StackTrace");
            propertiesToBeWritten.Remove("TargetSite");
            propertiesToBeWritten.Remove("Source");
        }
    }
}
