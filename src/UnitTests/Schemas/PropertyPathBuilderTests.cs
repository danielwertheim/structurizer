using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Structurizer;
using Structurizer.Schemas;

namespace UnitTests.Schemas
{
    [TestClass]
    public class PropertyPathBuilderTests : UnitTests
    {
        [TestMethod]
        public void BuildPath_ForRootMember_PathContainsOnlyName()
        {
            var property = new Mock<IStructureProperty>();
            property.Setup(s => s.IsRootMember).Returns(true);
            property.Setup(s => s.Name).Returns("TheName");
            
            var path = PropertyPathBuilder.BuildPath(property.Object);

            Assert.AreEqual("TheName", path);
        }

        [TestMethod]
        public void BuildPath_ForNestedMember_PathContainsRootAndDelimitorAndNameOfNested()
        {
            var rootProperty = new Mock<IStructureProperty>();
            rootProperty.Setup(s => s.IsRootMember).Returns(true);
            rootProperty.Setup(s => s.Name).Returns("TheRootMember");

            var nestedProperty = new Mock<IStructureProperty>();
            nestedProperty.Setup(s => s.IsRootMember).Returns(false);
            nestedProperty.Setup(s => s.Parent).Returns(rootProperty.Object);
            nestedProperty.Setup(s => s.Name).Returns("TheNestedProperty");

            var path = PropertyPathBuilder.BuildPath(nestedProperty.Object);

            Assert.AreEqual("TheRootMember.TheNestedProperty", path);
        }

        [TestMethod]
        public void BuildPath_ForRootMember_PathContainsNoRootButNestedNameAndMember()
        {
            var path = PropertyPathBuilder.BuildPath(null, "Temp");

            Assert.AreEqual("Temp", path);
        }

        [TestMethod]
        public void BuildPath_ForNestedMember_PathContainsNoRootButNestedNameAndMember()
        {
            var property = new Mock<IStructureProperty>();
            property.Setup(s => s.Path).Returns("Nested");

            var path = PropertyPathBuilder.BuildPath(property.Object, "Temp");

            Assert.AreEqual("Nested.Temp", path);
        }
    }
}