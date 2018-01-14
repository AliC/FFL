using FFL.UI.Web.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;
using Xunit;

namespace FFL.UI.Web.Mvc.Tests
{
    public class PlayerControllerTests
    {
        [Fact]
        public void ReturnsNonNullResult()
        {
            var playerController = new PlayerController();
            ViewResult result = playerController.Index() as ViewResult;

            Assert.NotNull(result);
        }
    }
}
