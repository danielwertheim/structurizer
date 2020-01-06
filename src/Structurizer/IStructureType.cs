using System;

namespace Structurizer
{
    public interface IStructureType
    {
        Type Type { get; }
        string Name { get; }
        IStructureProperty[] IndexableProperties { get; }
    }
}