using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zawody.Controllers;
using Zawody.Data;
using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace Zawody.Testy.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        /*private readonly ILogger<HomeController> _logger;*/
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeControllerTest(ApplicationDbContext context, IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            /*_logger = logger;*/
            _context = context;
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        /*[TestMethod]*/
        public void Index()
        {
            HomeController controller = new HomeController(_context, _configuration, _hostingEnvironment);

            ViewResult result = controller.Index() as ViewResult;

            Assert.IsNotNull(result);
        }

        /*[TestMethod]*/
        public void About()
        {
            HomeController controller = new HomeController(_context, _configuration, _hostingEnvironment);

            /*ViewResult result = controller.About() as ViewResult;

            Assert.AreEqual("", result);*/
        }
    }
}
