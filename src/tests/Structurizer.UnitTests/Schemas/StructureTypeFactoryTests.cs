using System.Collections.Generic;
using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Structurizer.UnitTests.Schemas
{
    [TestClass]
    public class StructureTypeFactoryTests : UnitTests
    {
        [TestMethod]
        public void Should_use_reflecter_to_get_all_indexable_properties_When_no_explicit_excludes_exists()
        {
            var typeConfigStub = new Mock<IStructureTypeConfig>();
            var typeConfig = typeConfigStub.Object;
            typeConfigStub.Setup(m => m.Type).Returns(typeof(MyClass));
            typeConfigStub.Setup(m => m.IndexMode).Returns(IndexMode.Exclusive);
            typeConfigStub.Setup(m => m.MemberPaths).Returns(new List<string>());

            var reflecterMock = new Mock<IStructureTypeReflecter>();

            var factory = new StructureTypeFactory(reflecterMock.Object);
            factory.CreateFor(typeConfig);

            reflecterMock.Verify(m => m.GetIndexableProperties(typeConfig.Type));
        }

        [TestMethod]
        public void Should_use_reflecter_to_get_indexable_properties_except_excluded_ones_When_excludes_exists()
        {
            var typeConfigStub = new Mock<IStructureTypeConfig>();
            var typeConfig = typeConfigStub.Object;
            typeConfigStub.Setup(m => m.Type).Returns(typeof(MyClass));
            typeConfigStub.Setup(m => m.IndexMode).Returns(IndexMode.Exclusive);
            typeConfigStub.Setup(m => m.MemberPaths).Returns(new List<string> { "FooBeingExcluded" });

            var reflecterMock = new Mock<IStructureTypeReflecter>();

            var factory = new StructureTypeFactory(reflecterMock.Object);
            factory.CreateFor(typeConfig);

            reflecterMock.Verify(m => m.GetIndexablePropertiesExcept(typeConfig.Type, It.IsAny<IList<string>>()));
        }

        [TestMethod]
        public void Should_use_reflecter_to_get_specific_indexable_properties_When_includes_exists()
        {
            var typeConfigStub = new Mock<IStructureTypeConfig>();
            var typeConfig = typeConfigStub.Object;
            typeConfigStub.Setup(m => m.Type).Returns(typeof(MyClass));
            typeConfigStub.Setup(m => m.IndexMode).Returns(IndexMode.Inclusive);
            typeConfigStub.Setup(m => m.MemberPaths).Returns(new List<string> { "FooBeingIncluded" });

            var reflecterMock = new Mock<IStructureTypeReflecter>();

            var factory = new StructureTypeFactory(reflecterMock.Object);
            factory.CreateFor(typeConfig);

            reflecterMock.Verify(m => m.GetSpecificIndexableProperties(typeConfig.Type, It.IsAny<IList<string>>()));
        }

        private class MyClass { }
    }
}