# Structurizer
[![NuGet](https://img.shields.io/nuget/v/structurizer.svg?cacheSeconds=3600)](https://www.nuget.org/packages/structurizer)
[![License MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://choosealicense.com/licenses/mit/)
[![Build Status](https://dev.azure.com/danielwertheim/structurizer/_apis/build/status/danielwertheim.structurizer-CI?branchName=master)](https://dev.azure.com/danielwertheim/structurizer/_build/latest?definitionId=31&branchName=master)

Structurizer is extracted from one of my other projects [PineCone](https://github.com/danielwertheim/pinecone), which was used for much of the underlying indexing stuff for another project of mine, SisoDB. Structurizer is reduced to only manage a key-value representation of an object-graph.

## Release notes
Release notes are [kept here](ReleaseNotes.md).

## Usage
### Define a model
You need some model to create key-values for. It will only extract public properties.

```csharp
public class MyRoot
{
    public string Name { get; set; }
    public int Score { get; set; }
    public MyChild OneChild { get; set; }
    public List<MyChild> ManyChildren { get; set; }
}

public class MyChild
{
    public string SomeString { get; set; }
}
```

### Install

```
install-package structurizer
```

### Builder construction
The easiest builder to use is the `FlexibleStructureBuilder` (introduced in `v3.0.0`).

```csharp
var builder = new FlexibleStructureBuilder();
```

You can also use the more static configured `StructureBuilder`:

```csharp
var typeConfigs = new StructureTypeConfigurations();
typeConfigs.Register<MyRoot>();

var builder = StructureBuilder.Create(typeConfigs);
```

### Create key-values
Now you can create a structure which will hold key-value `StructureIndex` items for the graph.

```csharp
var item = new MyRoot
{
    Name = "Foo Bar",
    Score = 2345,
    OneChild = new MyChild
    {
        SomeString = "One child"
    },
    ManyChildren = new List<MyChild>
    {
        new MyChild {SomeString = "List Child1"},
        new MyChild {SomeString = "List Child2"}
    }
};

var structure = builder.CreateStructure(item);
foreach (var index in structure.Indexes)
{
    Console.WriteLine($"{index.Path}={index.Value}");
}
```

will generate:

```
Name=Foo Bar
Score=2345
OneChild.SomeString=One child
ManyChildren[0].SomeString=List Child1
ManyChildren[1].SomeString=List Child2
```

## Control what's being indexed

### Using the FlexibleStructureBuilder
At any point (last in wins), just use the `Configure` methods, e.g.

```csharp
builder.Configure(i => cfg
    .UseIndexMode(IndexMode.Inclusive)
    .Members(e => e.Name, e => e.Score));
```

### Using the Static StructureBuilder
This is controlled using the `StructureTypeConfigurations.Register` member.

By default it's going to index everything as the default is to have `IndexMode.Exclusive` with no exclusions. This can be changed.

```csharp
var typeConfigs = new StructureTypeConfigurations();
typeConfigs.Register<MyRoot>(cfg => cfg
    .UseIndexMode(IndexMode.Inclusive)
    .Members(t => t.Name, t => t.Score)
    //Use array access to define path of childrens in enumerable
    .Members(t => t.ManyChildren[0].SomeString))
    //Can also be done using strings if no index property exists
    .Members("ManyChildren.SomeString");
```

# Use cases?
Anywhere where you efficiently need to extract key-values from an object-graph. Could e.g. be for:

- Logging
- Selectively extracting some property values for generating a checksum for an object-graph
- Comparing values
- Storing key-values for searching
- Creating a deltas for storing changes to entities in an audit database
- ...
