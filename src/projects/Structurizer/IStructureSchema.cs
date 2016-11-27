using System.Collections.Generic;

namespace Structurizer
{
    public interface IStructureSchema
    {
        IStructureType StructureType { get; }
        string Name { get; }
        IReadOnlyList<IIndexAccessor> IndexAccessors { get; }
    }
}