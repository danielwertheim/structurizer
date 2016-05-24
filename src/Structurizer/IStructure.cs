using System.Collections.Generic;

namespace Structurizer
{
    public interface IStructure
    {
        string Name { get; }
        IList<IStructureIndex> Indexes { get; }
    }
}