## About

The SmartDev.ConfigurationMapper is a small library that helps you map your ASP.NET 5 configuration to classes. It is useful when you want an easy way to access your config values strongly typed and with well defined default values.

NuGet: [![](http://img.shields.io/nuget/v/SmartDev.ConfigurationMapper.svg?style=flat-square)](http://www.nuget.org/packages/SmartDev.ConfigurationMapper) [![](http://img.shields.io/nuget/dt/SmartDev.ConfigurationMapper.svg?style=flat-square)](http://www.nuget.org/packages/SmartDev.ConfigurationMapper)

### Build status

CI is hosted on [AppVeyor](http://www.appveyor.com/).  
Project [/gingters/smartdev-configurationmapper](https://ci.appveyor.com/project/gingters/smartdev-configurationmapper).

Builds:
* master: [![Build status](https://ci.appveyor.com/api/projects/status/6xpyv803q7wawrd5/branch/master?svg=true&pendingText=master%20-%20pending&passingText=master%20-%20OK&failingText=master%20-%20failed)](https://ci.appveyor.com/project/gingters/smartdev-configurationmapper/branch/master)  
* dev: [![Build status](https://ci.appveyor.com/api/projects/status/6xpyv803q7wawrd5/branch/dev?svg=true&pendingText=dev%20-%20pending&passingText=dev%20-%20OK&failingText=dev%20-%20failed)](https://ci.appveyor.com/project/gingters/smartdev-configurationmapper/branch/dev)  

## Usage

### Most basic

Assume this `config.json`:

    {
        "SomeValue": "Hello World",
        "SomeIntValue": 42
    }

You define your configuration objects as POCOs. Make sure, your property names match the keys in the config. Like this:

    public class MyConfig
    {
        public string SomeValue { get; set; }
        public int SomeIntValue { get; set; }
    }

Based on the `IConfiguration` data, you can map the values to your class:

    // var config = an instance of IConfiguration, i.e. from DI
    var myConfig = config.Map<MyConfig>();

Access your config:

    // Will print 42 lines of 'Hello World'
    for (var i = 0; i < myConfig.SomeIntValue; i++)
    {
        Console.WriteLine(myConfig.SomeTextValue);
    }

### Alternative Keys

You can map your properties to other Key names by adding a Key attribute to it. So, given the same config, your can give the properties on your class more meaningful names:

    public class MyConfig
    {
        [Key("SomeValue")]
        public string TextToPrint { get; set; }
        [Key("SomeIntValue")]
        public int LinesToPrint { get; set; }
    }

### Scoping

In Microsoft.Framework.ConfigurationModel it is possible to scope your configuration down in a hiearchy. Either by nesting Json objects or by separating your key names with colons. You can scope your class to a certain Sub-key by either adding an attribute to your class:

    [Scope("Some:Sub:Key")]
    public class MyConfig
    {
        public string TextToPrint { get; set; }
        public int LinesToPrint { get; set; }
    }

Or by specifying the scope in the Map call:

    var myConfig = config.Map<MyConfig>("Sub:Key");


## Other stuff

### Interesting Background info on ConfigurationModel

* [ASP.NET 5 Moving Parts: IConfiguration](http://whereslou.com/2014/05/23/asp-net-vnext-moving-parts-iconfiguration/)
* [ASP.NET 5 Configuration - Microsoft.Framework.ConfigurationModel](http://blog.jsinh.in/asp-net-5-configuration-microsoft-framework-configurationmodel/)

### Dependencies

* [Microsoft.Framework.ConfigurationModel](https://github.com/aspnet/Configuration/)

### Rules for this project

* This project sticks to the corefx [Coding style](https://github.com/dotnet/corefx/wiki/Coding-style) with two important exceptions:  
  * Indentation is done with tabs. No arguments about that.
  * When calling methods on types, DON'T use the C# alias (i.e. `String.IsNullOrEmpty()` instead of `string.IsNullOrEmpty()`).


