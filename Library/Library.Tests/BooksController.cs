using NUnit.Framework;
using NUnit.Framework.Legacy;
using Library.Controllers;

namespace Library.Tests
{
    [TestFixture]
    public class BooksControllerTests
    {
        [Test]
        public void Controller_CanBeInstantiated()
        {
            // Act
            var controller = new BooksController(null);

            // Assert
            ClassicAssert.IsNotNull(controller, "The BooksController should be instantiated successfully.");
        }
    }
}
