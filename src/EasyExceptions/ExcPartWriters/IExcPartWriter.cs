﻿using System.Collections.Generic;
using System.Text;

namespace EasyExceptions.ExcPartWriters
{
    public interface IExcPartWriter
    {
        void Apply(StringBuilder resultBuilder, object obj, Dictionary<string, object> propertiesToBeWritten);
    }
}