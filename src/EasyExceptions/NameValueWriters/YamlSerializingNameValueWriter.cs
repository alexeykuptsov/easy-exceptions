using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using YamlDotNet.Serialization;

namespace EasyExceptions.NameValueWriters
{
    public class YamlSerializingNameValueWriter : NameValueWriterBase<object>
    {
        private static readonly ISerializer Serializer;

        static YamlSerializingNameValueWriter()
        {
            Serializer = new SerializerBuilder()
                .WithTargetInvocationExceptionsHandling()
                .Build();
        }

        protected override void WriteInternal(StringBuilder resultBuilder, string name, object value)
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

            var valueString = Serializer.Serialize(new Dictionary<string, object> { [name] = value });
            var lines = valueString.Split(new[] {Environment.NewLine}, StringSplitOptions.None);
            var nameValueSeparatorIndex = lines[0].IndexOf(": ", StringComparison.Ordinal);
            if (nameValueSeparatorIndex >= 0)
            {
                lines[0] = lines[0].Substring(0, nameValueSeparatorIndex) + " = " + lines[0].Substring(nameValueSeparatorIndex + 2);
            }
            else
            {
                lines[0] = lines[0].TrimEnd(':') + " =";
            }
            resultBuilder.Append(String.Join(Environment.NewLine, lines));
        }
    }
}