using SourceGenerator.Domain.Attributes;
using Xunit;

namespace SourceGenerator.Tests.AsyncMethods;

public class AsyncMethodTests
{
    [Fact]
    public void R()
    {
        var car = new Car();
    }
}

partial class Car
{
    [Async]
    public void Work(int w)
    {
    }
}