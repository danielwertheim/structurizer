using System;
using EnsureThat;

namespace Structurizer.Schemas
{
    [Serializable]
    public struct StructurePropertyInfo
    {
        public readonly IStructureProperty Parent;
        public readonly string Name;
        public readonly Type DataType;

        public StructurePropertyInfo(string name, Type dataType, IStructureProperty parent = null)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();
            Ensure.That(dataType, nameof(dataType)).IsNotNull();

            Parent = parent;
            Name = name;
            DataType = dataType;
        }
    }
}