using System.Collections.Generic;
using Structurizer.Schemas.MemberAccessors;

namespace Structurizer.Schemas
{
    public interface IStructureSchema
    {
        IStructureType Type { get; }
        string Name { get; }
        IList<IIndexAccessor> IndexAccessors { get; }
    }
}