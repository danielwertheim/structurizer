using System.Collections.Generic;

namespace Structurizer
{
    public interface IStructure
    {
        string Name { get; }
        IReadOnlyList<IStructureIndex> Indexes { get; }
    }
}