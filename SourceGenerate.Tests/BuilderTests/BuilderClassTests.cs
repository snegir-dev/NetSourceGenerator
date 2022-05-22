using SourceGenerate.Domain.Attributes;
using Xunit;

namespace SourceGenerate.Tests.BuilderTests;

public class BuilderClassTests
{
    [Fact]
    public void CarBuilderClassTest()
    {
        var car = CarBuilderClass.Builder().SetName("BMW").SetAge(7).Build();
        
        Assert.Equal("BMW", car.Name);
        Assert.Equal(7, car.Age);
    }
}

[Builder]
internal partial class CarBuilderClass
{
    public string Name { get; set; }
    public int Age { get; set; }
}