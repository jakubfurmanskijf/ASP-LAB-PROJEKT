using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Legacy;
using Library.Controllers;

namespace Library.Tests
{
    [TestFixture]
    public class HomeControllerTests
    {
        private HomeController _controller;

        [SetUp]
        public void Setup()
        {
            // Mock the ILogger dependency
            var mockLogger = new Mock<ILogger<HomeController>>();

            // Initialize HomeController with the mocked logger
            _controller = new HomeController(mockLogger.Object);

            // Mock HttpContext and set LastVisit
            var httpContext = new DefaultHttpContext();
            httpContext.Items["LastVisit"] = "2025-01-31 12:34:56";

            // Assign mocked HttpContext to the controller
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

        [TearDown]
        public void TearDown()
        {
            if (_controller is IDisposable disposableController)
            {
                disposableController.Dispose();
            }

            _controller = null; // Clean up the reference
        }

        [Test]
        public void Index_ReturnsViewResultWithCorrectLastVisit()
        {
            // Act
            var result = _controller.Index() as ViewResult;

            // Assert
            ClassicAssert.IsNotNull(result, "ViewResult should not be null.");
            ClassicAssert.IsInstanceOf<ViewResult>(result, "The result should be of type ViewResult.");

            // Verify LastVisit is passed to the view
            ClassicAssert.AreEqual("2025-01-31 12:34:56", result.ViewData["LastVisit"], "LastVisit value should match.");
        }
    }
}
