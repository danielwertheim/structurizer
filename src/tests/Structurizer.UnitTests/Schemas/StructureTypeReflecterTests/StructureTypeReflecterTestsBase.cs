using Microsoft.VisualStudio.TestTools.UnitTesting;
using Structurizer.Schemas;

namespace Structurizer.UnitTests.Schemas.StructureTypeReflecterTests
{
    [TestClass]
    public abstract class StructureTypeReflecterTestsBase : UnitTestBase
    {
        protected IStructureTypeReflecter ReflecterFor()
        {
            return new StructureTypeReflecter();
        }
    }
}