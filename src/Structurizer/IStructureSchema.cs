using System.Collections.Generic;
using Structurizer.Schemas.MemberAccessors;

namespace Structurizer
{
    public interface IStructureSchema
    {
        IStructureType StructureType { get; }
        string Name { get; }
        IReadOnlyList<IIndexAccessor> IndexAccessors { get; }
    }
}