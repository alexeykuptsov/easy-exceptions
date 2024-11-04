namespace EasyExceptions.Tests;

public class ThrowingPropertyException : Exception
{
    public object ThrowingProperty => throw new ObjectDisposedException("objectName1");

    public MyClass ObjectWithThrowingProperty => new();
        
    public class MyClass
    {
        public object ThrowingProperty => throw new ObjectDisposedException("objectName1");
    }
}