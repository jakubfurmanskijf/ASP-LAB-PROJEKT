using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Library.Tests
{
    [TestFixture]
    public class DummyTest
    {
        [Test]
        public void ForceNamespaceUsage()
        {
            ClassicAssert.IsTrue(true);
        }
    }
}