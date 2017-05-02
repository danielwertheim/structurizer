using System;
using EnsureThat;

namespace Structurizer.Schemas
{
    public class StructurePropertyInfo
    {
        public readonly string Name;
        public readonly Type DataType;
        public readonly IStructureProperty Parent;
        public readonly Attribute[] Attributes;

        public StructurePropertyInfo(
            string name,
            Type dataType,
            Attribute[] attributes,
            IStructureProperty parent = null)
        {
            EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));
            EnsureArg.IsNotNull(dataType, nameof(dataType));
            EnsureArg.IsNotNull(attributes, nameof(attributes));

            Name = name;
            DataType = dataType;
            Attributes = attributes;
            Parent = parent;
        }
    }
}