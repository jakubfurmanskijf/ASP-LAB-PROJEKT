using NUnit.Framework;
using NUnit.Framework.Legacy;
using Library.Controllers;

namespace Library.Tests
{
    [TestFixture]
    public class LoansControllerTests
    {
        [Test]
        public void Controller_CanBeInstantiated()
        {
            // Act
            var controller = new LoansController(null);

            // Assert
            ClassicAssert.IsNotNull(controller, "The LoansController should be instantiated successfully.");
        }
    }
}
