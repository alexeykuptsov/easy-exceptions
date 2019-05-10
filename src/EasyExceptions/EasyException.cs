using System;

namespace EasyExceptions
{
    public class EasyException : Exception
    {
        public EasyException(Exception innerException) : base(ExceptionDumpUtil.Dump(innerException), innerException)
        {}
    }
}