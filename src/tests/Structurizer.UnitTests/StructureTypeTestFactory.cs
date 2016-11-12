using Structurizer.Configuration;

namespace Structurizer.UnitTests
{
    internal static class StructureTypeTestFactory
    {
        internal static IStructureType CreateFor<T>() where T : class
        {
            var configs = new StructureTypeConfigurations();
            var factory = new StructureTypeFactory();

            var typeConfig = configs.Register<T>();

            return factory.CreateFor(typeConfig);
        }
    }
}