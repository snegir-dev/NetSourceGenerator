using System.Threading.Tasks;
using SourceGenerator.Domain.Attributes;
using Xunit;

namespace SourceGenerator.Tests.AsyncMethods;

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