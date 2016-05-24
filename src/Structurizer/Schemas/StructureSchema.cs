using System.Collections.Generic;
using EnsureThat;
using Structurizer.Schemas.MemberAccessors;

namespace Structurizer.Schemas
{
    public class StructureSchema : IStructureSchema
    {
        public IStructureType Type { get; }
        public string Name => Type.Name;
        public IList<IIndexAccessor> IndexAccessors { get; }

        public StructureSchema(IStructureType type, ICollection<IIndexAccessor> indexAccessors = null)
        {
            Ensure.That(type, "type").IsNotNull();

            Type = type;
            IndexAccessors = indexAccessors != null
                ? new List<IIndexAccessor>(indexAccessors)
                : new List<IIndexAccessor>();
        }
    }
}