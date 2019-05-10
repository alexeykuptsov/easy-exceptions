using System;

namespace EasyExceptions.Tests
{
    public class FallingException : Exception
    {
        public object FallingProperty
        {
            get
            {
                throw new NotSupportedException();
            }
        }
    }
}