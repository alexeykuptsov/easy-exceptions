using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyExceptions.NameValueWriters;

namespace EasyExceptions.WritingRules
{
    public class RegularPropertiesRule : IWritingRule
    {
        public static List<IPropertyWriter> NameValueWriters => new List<IPropertyWriter>
        {
            new DbEntityValidationResultPropertyWriter(),
            new DbValidationErrorPropertyWriter(),
            new ExceptionPropertyWriter(),
            new DictionaryPropertyWriter(),
            new EnumerablePropertyWriter(),
            new SimplePropertyWriter()
        };


        public void Apply(StringBuilder resultBuilder, object obj, Dictionary<string, object> allProperties)
        {
            foreach (var propertyNameAndValue in allProperties)
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
            foreach (var writer in NameValueWriters.Where(_ => _.IsValueApplicable(value)))
            {
                writer.Write(resultBuilder, name, value);
                break;
            }
        }
    }
}