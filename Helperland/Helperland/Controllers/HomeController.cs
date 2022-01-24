using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Helperland.Controllers
{
    [Route("[controller]")]
    public class HomeController : Controller
    {
        [Route("")]
        //[Route("Index")]
        [Route("[action]")]
        [Route("~/")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("[action]")]
        public IActionResult Price()
        {
            return View();
        }
        [Route("[action]")]
        public IActionResult Faq() 
        {
            return View();
        }
        [Route("[action]")]
        public IActionResult Aboutus()
        {
            return View();
        }
        [Route("[action]")]
        public IActionResult Contactus()
        {
            return View();
        }

    }
}
