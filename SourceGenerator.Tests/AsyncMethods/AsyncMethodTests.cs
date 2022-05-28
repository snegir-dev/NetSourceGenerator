using SourceGenerator.Domain.Attributes;
using Xunit;

namespace SourceGenerator.Tests.AsyncMethods;

public class AsyncMethodTests
{
    private Car _car = new();
    
    [Fact]
    public async void ReturnVoidMethodTest()
    {
        // var result = await _car.ReturnVoidMethodAsync("");
        //
        // Assert.Equal("Work", result);
    }
    
    [Fact]
    public async void ReturnStringMethodTest()
    {
        var result = await _car.ReturnStringMethodAsync();
        
        Assert.Equal("Work", result);
    }
}

internal partial class Car
{
    [Async]
    public void ReturnVoidMethod(out string s)
    {
        s = "work";
    }
    
    [Async]
    public string ReturnStringMethod()
    {
        return "Work";
    }
}