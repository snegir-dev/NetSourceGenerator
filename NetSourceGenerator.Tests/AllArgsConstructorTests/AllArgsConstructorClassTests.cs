using NetSourceGenerator.Domain.Attributes;
using NetSourceGenerator.Domain.Enum;
using Xunit;

namespace NetSourceGenerator.Tests.AllArgsConstructorTests;

public class AllArgsConstructorClassTests
{
    [Fact]
    private void AllArgsConstructorTest()
    {
        var car = new CarAllArgsConstructorClass("BMW", 5);
        
        Assert.Equal("BMW", car.Brand);
        Assert.Equal(5, car.Age);
    }

    [Fact]
    private void ConstructorWithOnlyPropertyTest()
    {
        var car = new CarWithOnlyPropertyClass("BMW");
        
        Assert.Equal("BMW", car.Brand);
        Assert.Equal(0, car.Age);
    }

    [Fact]
    private void ConstructorWithOnlyFieldTest()
    {
        var car = new CarWithOnlyFieldClass(5);
        
        Assert.Null(car.Brand);
        Assert.Equal(5, car.Age);
    }

    [Fact]
    private void ConstructorWithOnlyInternalModifierTest()
    {
        var car = new CarWithOnlyInternalModifierClass("BMW");
        
        Assert.Equal("BMW", car.Brand);
        Assert.Equal(0, car.Age);
    }

    [Fact]
    private void ConstructorWithOnlyPrivateModifierTest()
    {
        var car = new CarWithOnlyPrivateModifierClass("BMW");
        
        Assert.Equal("BMW", car.Brand);
        Assert.Equal(0, car.Age);
    }
    
    [Fact]
    private void ConstructorWithOnlyProtectedModifierTest()
    {
        var car = new CarWithOnlyProtectedModifierClass("BMW");
        
        Assert.Equal("BMW", car.Brand);
        Assert.Equal(0, car.Age);
    }
}

[AllArgsConstructor]
public partial class CarAllArgsConstructorClass
{
    public string Brand { get; set; }
    public int Age;
}

[AllArgsConstructor(MemberType.Property)]
public partial class CarWithOnlyPropertyClass
{
    public string Brand { get; set; }
    public int Age;
}

[AllArgsConstructor(MemberType.Field)]
public partial class CarWithOnlyFieldClass
{
    public string Brand { get; set; }
    public int Age;
}

[AllArgsConstructor(AccessType.Internal)]
public partial class CarWithOnlyInternalModifierClass
{
    internal string Brand { get; set; }
    public int Age;
}


[AllArgsConstructor(AccessType.Private)]
partial class CarWithOnlyPrivateModifierClass
{
    private string _brand;
    public int Age;

    public string Brand => _brand;
}

[AllArgsConstructor(AccessType.Protected)]
public partial class CarWithOnlyProtectedModifierClass
{
    protected string _brand;
    public int Age;

    public string Brand => _brand;
}