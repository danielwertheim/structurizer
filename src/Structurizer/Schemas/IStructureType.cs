using System;

namespace Structurizer.Schemas
{
    public interface IStructureType
    {
        Type Type { get; }
        string Name { get; }
        IStructureProperty[] IndexableProperties { get; }
    }
}