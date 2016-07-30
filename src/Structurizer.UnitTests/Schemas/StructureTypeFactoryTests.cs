using System.Collections.Generic;
using Moq;
using NUnit.Framework;

namespace Structurizer.UnitTests.Schemas
{
    [TestFixture]
    public class StructureTypeFactoryTests
    {
        [Test]
        public void CreateFor_WhenNoExplicitConfigExistsForType_InvokesGetIndexablePropertiesOnReflecter()
        {
            var typeConfigStub = new Mock<IStructureTypeConfig>();
            var typeConfig = typeConfigStub.Object;
            typeConfigStub.Setup(m => m.Type).Returns(typeof(MyClass));
            typeConfigStub.Setup(m => m.IndexConfigIsEmpty).Returns(true);

            var reflecterMock = new Mock<IStructureTypeReflecter>();

            var factory = new StructureTypeFactory(reflecterMock.Object);
            factory.CreateFor(typeConfig);

            reflecterMock.Verify(m => m.GetIndexableProperties(typeConfig.Type));
        }

        [Test]
        public void CreateFor_WhenConfigExcludingMembersExistsForType_InvokesGetIndexablePropertiesExceptOnReflecter()
        {
            var typeConfigStub = new Mock<IStructureTypeConfig>();
            var typeConfig = typeConfigStub.Object;
            typeConfigStub.Setup(m => m.Type).Returns(typeof(MyClass));
            typeConfigStub.Setup(m => m.IndexConfigIsEmpty).Returns(false);
            typeConfigStub.Setup(m => m.MemberPathsNotBeingIndexed).Returns(() => new HashSet<string> { "ExcludeTEMP" });

            var reflecterMock = new Mock<IStructureTypeReflecter>();

            var factory = new StructureTypeFactory(reflecterMock.Object);
            factory.CreateFor(typeConfig);

            reflecterMock.Verify(m => m.GetIndexablePropertiesExcept(typeConfig.Type, new[] { "ExcludeTEMP" }));
        }

        [Test]
        public void CreateFor_WhenConfigIncludingMembersExistsForType_InvokesGetSpecificIndexablePropertiesOnReflecter()
        {
            var typeConfigStub = new Mock<IStructureTypeConfig>();
            var typeConfig = typeConfigStub.Object;
            typeConfigStub.Setup(m => m.Type).Returns(typeof(MyClass));
            typeConfigStub.Setup(m => m.IndexConfigIsEmpty).Returns(false);
            typeConfigStub.Setup(m => m.MemberPathsNotBeingIndexed).Returns(() => new HashSet<string>());
            typeConfigStub.Setup(m => m.MemberPathsBeingIndexed).Returns(() => new HashSet<string> { "IncludeTEMP" });

            var reflecterMock = new Mock<IStructureTypeReflecter>();

            var factory = new StructureTypeFactory(reflecterMock.Object);
            factory.CreateFor(typeConfig);

            reflecterMock.Verify(m => m.GetSpecificIndexableProperties(typeConfig.Type, new[] { "IncludeTEMP" }));
        }

        private class MyClass { }
    }
}