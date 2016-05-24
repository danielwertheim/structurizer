using Moq;
using NUnit.Framework;
using Structurizer.Schemas;

namespace Structurizer.UnitTests.Schemas
{
    [TestFixture]
    public class StructureTypeFactoryTests
    {
        [Test]
        public void CreateFor_WhenNoExplicitConfigExistsForType_InvokesGetIndexablePropertiesOnReflecter()
        {
            var type = typeof(MyClass);
            var reflecterMock = new Mock<IStructureTypeReflecter>();
            reflecterMock.Setup(m => m.GetIndexableProperties(type)).Returns(() =>
            {
                var indexProperty = new Mock<IStructureProperty>();

                return new[] { indexProperty.Object };
            });

            var factory = new StructureTypeFactory(t => reflecterMock.Object);
            factory.CreateFor(type);

            reflecterMock.Verify(m => m.GetIndexableProperties(type));
        }

        [Test]
        public void CreateFor_WhenConfigExcludingMembersExistsForType_InvokesGetIndexablePropertiesExceptOnReflecter()
        {
            var type = typeof(MyClass);
            var reflecterMock = new Mock<IStructureTypeReflecter>();
            reflecterMock.Setup(m => m.GetIndexablePropertiesExcept(type, new[] { "ExcludeTEMP" })).Returns(() =>
             {
                 var indexProperty = new Mock<IStructureProperty>();

                 return new[] { indexProperty.Object };
             });

            var factory = new StructureTypeFactory(t => reflecterMock.Object);
            factory.Configurations.Configure(type, cfg => cfg.DoNotIndexThis("ExcludeTEMP"));
            factory.CreateFor(type);

            reflecterMock.Verify(m => m.GetIndexablePropertiesExcept(type, new[] { "ExcludeTEMP" }));
        }

        [Test]
        public void CreateFor_WhenConfigIncludingMembersExistsForType_InvokesGetSpecificIndexablePropertiesOnReflecter()
        {
            var type = typeof(MyClass);
            var reflecterMock = new Mock<IStructureTypeReflecter>();
            reflecterMock.Setup(m => m.GetSpecificIndexableProperties(type, new[] { "IncludeTEMP" })).Returns(() =>
            {
                var indexProperty = new Mock<IStructureProperty>();

                return new[] { indexProperty.Object };
            });

            var factory = new StructureTypeFactory(t => reflecterMock.Object);
            factory.Configurations.Configure(type, cfg => cfg.OnlyIndexThis("IncludeTEMP"));
            factory.CreateFor(type);

            reflecterMock.Verify(m => m.GetSpecificIndexableProperties(type, new[] { "IncludeTEMP" }));
        }

        private class MyClass { }
    }
}