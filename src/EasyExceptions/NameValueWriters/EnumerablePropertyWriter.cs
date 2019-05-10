using System.Collections;
using System.Text;
using EasyExceptions.WritingRules;

namespace EasyExceptions.NameValueWriters
{
    public class EnumerablePropertyWriter : PropertyWriterBase<IEnumerable>
    {
        public override bool IsValueApplicable(object value)
        {
            if (value is string)
                return false;
            return base.IsValueApplicable(value);
        }

        protected override void WriteInternal(StringBuilder resultBuilder, string name, IEnumerable enumerable)
        {
            int i = 0;
            foreach (var item in enumerable)
            {
                string combinedName = name + "[" + i + "]";
                RegularPropertiesRule.WriteNameValue(resultBuilder, combinedName, item);
                i++;
            }
        }
    }
}