using System.Collections;
using System.Linq;
using System.Text;
using EasyExceptions.WritingRules;

namespace EasyExceptions.NameValueWriters
{
    public class DictionaryPropertyWriter : PropertyWriterBase<IDictionary>
    {
        protected override void WriteInternal(StringBuilder resultBuilder, string name, IDictionary dictionary)
        {
            var sortedKeys = dictionary.Keys.Cast<object>()
                .Select(_ => new { Key = _, ToStr = _.ToString() })
                .OrderBy(_ => _.ToStr)
                .Select(_ => _.Key);
            foreach (var key in sortedKeys)
            {
                string combinedName;
                var keyString = key as string;
                if (keyString != null)
                {
                    if (keyString.StartsWith(ExceptionDumpUtil.ServiceDataPrefix))
                        continue;
                    combinedName = name + "[\"" + key + "\"]";
                }
                else
                    combinedName = name + "[" + key + "]";

                var value = dictionary[key];
                RegularPropertiesRule.WriteNameValue(resultBuilder, combinedName, value);
            }
        }
    }
}