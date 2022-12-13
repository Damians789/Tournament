using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zawody.Controllers;
using Zawody.Data;
using Zawody.Services;

namespace Testy.Controllers
{
    [TestClass]
    public class AddressesControllerTest
    {
        private readonly ApplicationDbContext _context;
        
        /*[TestMethod]*/
        public void Index()
        {
            AddressesController controller = new AddressesController(_context);

            ViewResult result = (ViewResult)(controller.Index() as IViewComponentResult);

            Assert.IsNotNull(result);
        }
    }
}
