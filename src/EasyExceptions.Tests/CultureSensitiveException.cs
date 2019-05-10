using System;
using System.Collections.Generic;

namespace EasyExceptions.Tests
{
    public class CultureSensitiveException : Exception
    {
        public DateTime DateTime { get; } = new DateTime(2016, 10, 30, 23, 50, 19, DateTimeKind.Utc);

        public double Double { get; } = 3.14159265359;
    }
}