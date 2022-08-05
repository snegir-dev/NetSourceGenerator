using NetSourceGenerator.Domain.Attributes;
using Xunit;

namespace NetSourceGenerator.Tests.SingletonTests;

public class SingletonClassTests
{
    [Fact]
    public void CarSingletonClassTest()
    {
        var car1 = CarSingletonClass.GetInstance();
        var car2 = CarSingletonClass.GetInstance();
        
        Assert.Equal(car1, car2);
    }
}

[Singleton]
internal partial class CarSingletonClass
{
    public string Name { get; set; }
    public int Age { get; set; }
}