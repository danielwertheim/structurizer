using NUnit.Framework;
using Structurizer.Schemas;

namespace Structurizer.UnitTests.Schemas.StructureTypeReflecterTests
{
    [TestFixture]
    public abstract class StructureTypeReflecterTestsBase : UnitTestBase
    {
        protected IStructureTypeReflecter ReflecterFor()
        {
            return new StructureTypeReflecter();
        }
    }
}