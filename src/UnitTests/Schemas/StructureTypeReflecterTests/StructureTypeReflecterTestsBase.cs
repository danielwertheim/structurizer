using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structurizer;
using Structurizer.Schemas;

namespace UnitTests.Schemas.StructureTypeReflecterTests
{
    [TestClass]
    public abstract class StructureTypeReflecterTestsBase : UnitTests
    {
        protected IStructureTypeReflecter ReflecterFor()
        {
            return new StructureTypeReflecter();
        }
    }
}