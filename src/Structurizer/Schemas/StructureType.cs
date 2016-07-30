using System;
using EnsureThat;

namespace Structurizer.Schemas
{
    public class StructureType : IStructureType
    {
        public Type Type { get; }
        public string Name => Type.Name;
        public IStructureProperty[] IndexableProperties { get; }

        public StructureType(
            Type type,
            IStructureProperty[] indexableProperties = null)
        {
            Ensure.That(type, "type").IsNotNull();

            Type = type;
            IndexableProperties = indexableProperties ?? new IStructureProperty[] { };
        }
    }
}