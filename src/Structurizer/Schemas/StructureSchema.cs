using System.Collections.Generic;
using EnsureThat;

namespace Structurizer.Schemas
{
    public class StructureSchema : IStructureSchema
    {
        public IStructureType StructureType { get; }
        public string Name => StructureType.Name;
        public IReadOnlyList<IIndexAccessor> IndexAccessors { get; }

        public StructureSchema(IStructureType structureType, ICollection<IIndexAccessor> indexAccessors = null)
        {
            Ensure.That(structureType, nameof(structureType)).IsNotNull();

            StructureType = structureType;
            IndexAccessors = indexAccessors != null
                ? new List<IIndexAccessor>(indexAccessors)
                : new List<IIndexAccessor>();
        }
    }
}