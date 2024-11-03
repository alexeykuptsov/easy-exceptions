using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyExceptions.ExcPartWriters
{
    public class RegularPropertiesWriter : IExcPartWriter
    {
        public void Apply(StringBuilder resultBuilder, object obj, Dictionary<string, object> propertiesToBeWritten)
        {
            foreach (var propertyNameAndValue in propertiesToBeWritten)
            {
                var value = propertyNameAndValue.Value;
                if (value == null)
                    continue;
                var name = propertyNameAndValue.Key;
                WriteNameValue(resultBuilder, name, value);
            }
        }

        public static void WriteNameValue(StringBuilder resultBuilder, string name, object value)
        {
            foreach (var writer in ExceptionDumpUtil.NameValueWriters.Where(_ => _.IsValueApplicable(value)))
            {
                writer.Write(resultBuilder, name, value);
                break;
            }
        }
    }
}