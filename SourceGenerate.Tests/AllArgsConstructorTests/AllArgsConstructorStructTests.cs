using SourceGenerate.Domain.Attributes;
using SourceGenerate.Domain.Enum;
using Xunit;

namespace SourceGenerate.Tests.AllArgsConstructorTests;

public class AllArgsConstructorStructTests
{
    [Fact]
    private void AllArgsConstructorTest()
    {
        var car = new CarAllArgsConstructorStruct("BMW", 5);

        Assert.Equal("BMW", car.Brand);
        Assert.Equal(5, car.Age);
    }
}

[AllArgsConstructor]
public partial struct CarAllArgsConstructorStruct
{
    public string Brand { get; set; }
    public int Age;
}