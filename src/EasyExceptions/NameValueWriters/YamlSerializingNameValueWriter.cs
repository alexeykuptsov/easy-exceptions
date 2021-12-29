using System.Collections;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace EasyExceptions.NameValueWriters
{
    public class YamlSerializingNameValueWriter : NameValueWriterBase<object>
    {
        private readonly ISerializer mySerializer;

        public YamlSerializingNameValueWriter()
        {
            mySerializer = new SerializerBuilder().Build();
        }

        protected override void WriteInternal(StringBuilder resultBuilder, string name, object value)
        {
            if (value is IEnumerable)
            {
                if (value is IDictionary dict)
                {
                    var dictToWrite = new Dictionary<object, object>();
                    foreach (DictionaryEntry dictionaryEntry in dict)
                    {
                        if (dictionaryEntry.Key is string keyString && keyString.StartsWith(ExceptionDumpUtil.ServiceDataPrefix)) 
                            continue;
                        dictToWrite[dictionaryEntry.Key] = dictionaryEntry.Value;
                    }
                    if (dictToWrite.Count == 0)
                        return;
                    value = dictToWrite;
                }
                var valueString = mySerializer.Serialize(new Dictionary<string, object> { [name] = value });
                resultBuilder.Append(valueString);
            }
            else
            {
                resultBuilder.AppendFormat("{0} = {1}", name, mySerializer.Serialize(value));
            }
        }
    }
}