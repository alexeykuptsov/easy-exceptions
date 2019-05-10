using System;

namespace EasyExceptions.Tests
{
    public class DynamicException : Exception
    {
        public Exception DynamicInnerException => new DynamicException();
    }
}