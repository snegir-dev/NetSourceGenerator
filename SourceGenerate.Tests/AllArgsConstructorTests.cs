using SourceGenerate.Domain.Attributes;
using SourceGenerate.Domain.Enum;
using Xunit;

namespace SourceGenerate.Tests;

public class AllArgsConstructorTests
{
    [Fact]
    private void AllArgsConstructorTest()
    {
        var car = new CarAllArgsConstructor("BMW", 5);
        
        Assert.Equal("BMW", car.Brand);
        Assert.Equal(5, car.Age);
    }

    [Fact]
    private void ConstructorWithOnlyPropertyTest()
    {
        var car = new CarWithOnlyProperty("BMW");
        
        Assert.Equal("BMW", car.Brand);
        Assert.Equal(0, car.Age);
    }

    [Fact]
    private void ConstructorWithOnlyFieldTest()
    {
        var car = new CarWithOnlyField(5);
        
        Assert.Null(car.Brand);
        Assert.Equal(5, car.Age);
    }

    [Fact]
    private void ConstructorWithOnlyInternalModifierTest()
    {
        var car = new CarWithOnlyInternalModifier("BMW");
        
        Assert.Equal("BMW", car.Brand);
        Assert.Equal(0, car.Age);
    }

    [Fact]
    private void ConstructorWithOnlyPrivateModifierTest()
    {
        var car = new CarWithOnlyPrivateModifier("BMW");
        
        Assert.Equal("BMW", car.Brand);
        Assert.Equal(0, car.Age);
    }
    
    [Fact]
    private void ConstructorWithOnlyProtectedModifierTest()
    {
        var car = new CarWithOnlyProtectedModifier("BMW");
        
        Assert.Equal("BMW", car.Brand);
        Assert.Equal(0, car.Age);
    }
}

[AllArgsConstructor]
public partial class CarAllArgsConstructor
{
    public string Brand { get; set; }
    public int Age;
}

[AllArgsConstructor(MemberType.Property)]
public partial class CarWithOnlyProperty
{
    public string Brand { get; set; }
    public int Age;
}

[AllArgsConstructor(MemberType.Field)]
public partial class CarWithOnlyField
{
    public string Brand { get; set; }
    public int Age;
}

[AllArgsConstructor(AccessType.Internal)]
public partial class CarWithOnlyInternalModifier
{
    internal string Brand { get; set; }
    public int Age;
}


[AllArgsConstructor(AccessType.Private)]
public partial class CarWithOnlyPrivateModifier
{
    private string _brand;
    public int Age;

    public string Brand => _brand;
}

[AllArgsConstructor(AccessType.Protected)]
public partial class CarWithOnlyProtectedModifier
{
    protected string _brand;
    public int Age;

    public string Brand => _brand;
}



