# Structurizer
Structurizer is extracted from one of my other projects [PineCone](https://github.com/danielwertheim/pinecone), which was used for much of the underlying indexing stuff for another project of mine, SisoDB. Structurizer is reduced to only manage a key-value representation of an object-graph.

## Release notes
Release notes are [kept here](ReleaseNotes.md).

## Usage
You need some model:

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

Then you can create a structure which will hold key-value `StructureIndex` items for the graph.

**KEEP HOLD OF THE SCHEMAS!** The Scheams contains a cache of IL-generated members for effective extraction of values.

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

var schemas = new StructureSchemas(new StructureTypeFactory(), new AutoStructureSchemaBuilder());
var schema = schemas.GetSchema<MyRoot>();
var builder = new StructureBuilder();
var structure = builder.CreateStructure(item, schema);
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
ManyChildren.SomeString=List Child1
ManyChildren.SomeString=List Child2
```
