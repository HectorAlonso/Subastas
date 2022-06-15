using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Subasta.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult NoEncontrado()
        {
            return View();
        }
    }
}