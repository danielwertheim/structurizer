using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests
{
    [TestClass]
    public abstract class UnitTests {}

    [TestClass]
    public abstract class UnitTestsOf<T> : UnitTests
    {
        public T UnitUnderTest { get; set; }
    }
}