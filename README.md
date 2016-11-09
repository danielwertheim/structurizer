# Structurizer
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

### Builder construction
**KEEP HOLD OF THE STRUCTUREBUILDER!** The `StructureBuilder.Create` function, will create the `Schemas` underneath. The Scheams contains a cache of IL-generated members for effective extraction of values.

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
