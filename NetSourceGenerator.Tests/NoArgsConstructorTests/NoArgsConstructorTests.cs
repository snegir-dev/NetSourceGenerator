﻿using NetSourceGenerator.Domain.Attributes;
using Xunit;

namespace NetSourceGenerator.Tests.NoArgsConstructorTests;

public class NoArgsConstructorTests
{
    [Fact]
    private void Test1()
    {
        var car = new CarWithNoArgsConstructor();
        
        Assert.NotNull(car);
    }
}

[NoArgsConstructor]
internal partial class CarWithNoArgsConstructor
{
    private string _name;
    private int _age;

    public CarWithNoArgsConstructor(string name, int age)
    {
        _name = name;
        _age = age;
    }
}