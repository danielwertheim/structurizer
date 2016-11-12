namespace Structurizer.UnitTests
{
    internal static class StructureSchemaTestFactory
    {
        internal static IStructureSchema CreateRealFrom<T>() where T : class
        {
            var structureType = StructureTypeTestFactory.CreateFor<T>();
            var factory = new StructureSchemaFactory();

            return factory.CreateSchema(structureType);
        }
    }
}