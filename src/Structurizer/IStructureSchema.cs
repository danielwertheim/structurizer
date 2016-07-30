using System.Collections.Generic;
using Structurizer.Schemas.MemberAccessors;

namespace Structurizer
{
    public interface IStructureSchema
    {
        IStructureType Type { get; }
        string Name { get; }
        IList<IIndexAccessor> IndexAccessors { get; }
    }
}