using FluentAssertions;
using Library.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace Library.Tests.Controllers
{
    [TestFixture]
    public class HomeControllerShould 
    {
        [Test]
        public void Return_Home_Page()
        {
            var controller = new HomeController();
            var result = controller.Index();
            result.Should().BeOfType<ViewResult>();
        }
    }
}
