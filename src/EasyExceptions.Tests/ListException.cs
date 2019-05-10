using System;
using System.Collections.Generic;

namespace EasyExceptions.Tests
{
    public class ListException : Exception
    {
        public List<object> List => new List<object> {"foo", new[] {"bar", "buz"}};
    }
}