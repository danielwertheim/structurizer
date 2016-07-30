using System;
using Structurizer.Schemas;

namespace Structurizer.UnitTests
{
    internal static class StructureSchemaTestFactory
    {
        private static readonly IStructureTypeFactory StructureTypeFactory = new StructureTypeFactory();
        private static readonly IStructureSchemaFactory StructureSchemaFactory = new StructureSchemaFactory();

        internal static IStructureSchema CreateRealFrom<T>() where T : class
        {
            return StructureSchemaFactory.CreateSchema(StructureTypeFactory.CreateFor<T>());
        }

        internal static IStructureSchema CreateRealFrom(Type type)
        {
            return StructureSchemaFactory.CreateSchema(StructureTypeFactory.CreateFor(type));
        }

        internal static IStructureSchema CreateRealFrom(IStructureType structureType)
        {
            return StructureSchemaFactory.CreateSchema(structureType);
        }
    }
}