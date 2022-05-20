using System;

namespace EasyExceptions.Tests
{
    public class ReferenceLoopObjectException : Exception
    {
        public ReferenceLoopObject ReferenceLoopObject { get; } = new ReferenceLoopObject();
    }
}