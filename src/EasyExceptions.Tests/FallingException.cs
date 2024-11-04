namespace EasyExceptions.Tests;

public class FallingException : Exception
{
    public object FallingProperty => throw new NotSupportedException();
}