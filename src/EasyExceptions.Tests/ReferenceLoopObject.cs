namespace EasyExceptions.Tests;

public class ReferenceLoopObject
{
    public ReferenceLoopObject LoopedReference => this;
    public string Text => "Hello";
}