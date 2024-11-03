using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using EasyExceptions.ExcPartWriters;

namespace EasyExceptions.NameValueWriters
{
    public class InnerExceptionsNameValueWriter : NameValueWriterBase<IEnumerable>
    {
        public override bool IsValueApplicable(object value)
        {
            return value is IEnumerable<Exception> && base.IsValueApplicable(value);
        }

        protected override void WriteInternal(StringBuilder resultBuilder, string name, IEnumerable enumerable)
        {
            int i = 0;
            foreach (var item in enumerable)
            {
                string combinedName = name + "[" + i + "]";
                RegularPropertiesWriter.WriteNameValue(resultBuilder, combinedName, item);
                i++;
            }
        }
    }
}