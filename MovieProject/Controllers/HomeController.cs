using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MovieProject.Controllers
{
    //controller of home page
    public class HomeController : Controller
    {

        //return the home page
        public ActionResult Index()
        {
            return View();
        }

        //return the news page
        public ActionResult News()
        {
            return View();
        }
    }
}