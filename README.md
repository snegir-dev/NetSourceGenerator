# SourceGenerator - Netstandard2.0 #

## Links

* [Description](#Description)
* [Start](#Start)
* [Documentation](#Documentation)
    * [Singleton](#Singleton)
    * [Builder](#Builder)
    * [AllArgsConstructor](#AllArgsConstructor)
    * [NoArgsConstructorAttribute](#NoArgsConstructor)
    * [Async](#Async)
* [Problems](#Problems)
* [Contacts](#Contacts)

## Description

The library is built on the .NET Compiler Platform (Roslyn), the library allows you to generate code based on
attributes.
The library has code analyzers and code fixes that detect and fix code errors.

## Start

* To use the library download it from [NuGet](https://www.nuget.org/packages/NetSourceGenerator).

OR

* You need to download the project from GitHub and connect it to the desired project using the link.

```xml
<ItemGroup>
        <ProjectReference Include="..\NetSourceGenerator\NetSourceGenerator.csproj" 
                          PrivateAssets="contentfiles;build" 
                          OutputItemType="Analyzer" 
                          ReferenceOutputAssembly="true" />
</ItemGroup>
```

## Documentation

### `Singleton`

The `Singleton` attribute generates a Singleton pattern for the class. 
This attribute creates a partial class with a method by which you can access a class object.

Sample code

```C#
public static class Program
{
    public static void Main(string[] args)
    {
        var color = Color.GetInstance();
    }
}

[Singleton]
public partial class Color
{
    public string Name { get; set; }
}
```

------------------------------------------------

### `Builder`

The `Builder` attribute allows you to create a builder fluent pattern based on the class or structure on which it was
hung.
The attribute generates a nested class with value assignment methods for properties and fields.

Sample code

```C#
public static class Program
{
    public static void Main(string[] args)
    {
        var color = Color.Builder()
            .SetName("red")
            .Build();
    }
}

[Builder]
public partial class Color
{
    public string Name { get; set; }
}
```

------------------------------------------------

### `AllArgsConstructor`

The `AllArgsConstructor` attribute generates a constructor with all properties for a class or structure.
The attribute can also take two parameter `MemberType` and `AccessType` into the constructor.

`MemberType` - Specifies what will be in the constructor.

`AccessType` - Specifies the access modifier for the fields and properties that will be in the designer.

Sample code

```C#
public static class Program
{
    public static void Main(string[] args)
    {
        var color = new Color("red");
    }
}

[AllArgsConstructor]
public partial class Color
{
    public string Name { get; set; }
}
```

------------------------------------------------

### `NoArgsConstructor`

The `NoArgsConstructor` attribute allows you to create an empty constructor.

Sample code

```C#
public static class Program
{
    public static void Main(string[] args)
    {
        var color = new Color();
    }
}

[NoArgsConstructor]
public partial class Color
{
    public string Name { get; set; }
    
    public Color(string name)
    {
        Name = name;
    }
}
```

------------------------------------------------

### `Async`

The `Async` attribute generates asynchronous method based on synchronous method.

Sample code

```C#
public static class Program
{
    public static async Task Main(string[] args)
    {
        var color = new Color();
        var code = await color.GetCodeAsync();
    }
}

public partial class Color
{
    [Async]
    public int GetCode()
    {
        return 123;
    }
}
```

## Problems

Sometimes, after deleting an attribute, the generated code may remain, to fix this error, you need to put `[]` where the attribute was and delete them.

## Contacts

* GitHub - https://github.com/SnEG1R
* LinkedIn - https://www.linkedin.com/in/evgeny-shaporov
