namespace EasyExceptions.Tests;

public class ListException : Exception
{
    public List<object> List => new() {"foo", new[] {"bar", "buz"}};
}