using NetSourceGenerator.Domain.Attributes;
using Xunit;

namespace NetSourceGenerator.Tests.AsyncMethods;

public class AsyncMethodTests
{
    private readonly Car _car = new();

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
    public string ReturnStringMethod()
    {
        return "Work";
    }
}