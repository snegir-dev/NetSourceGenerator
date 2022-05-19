using SourceGenerate.Domain.Attributes;
using Xunit;

namespace SourceGenerate.Tests.BuilderTests;

public class BuilderStructTests
{
    [Fact]
    public void CarBuilderStructTest()
    {
        var car = CarBuilderClass.Builder().SetName("BMW").SetAge(7).Build();
        
        Assert.Equal("BMW", car.Name);
        Assert.Equal(7, car.Age);
    }
}

[Builder]
internal partial struct CarBuilderStruct
{
    public string Name { get; set; }
    public int Age { get; set; }
}