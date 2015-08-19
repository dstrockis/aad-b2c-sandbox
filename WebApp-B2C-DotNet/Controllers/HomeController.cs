using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace WebApp_B2C_DotNet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.HttpMethod == "POST") 
            {
                return new RedirectResult("/");
            }

            ViewData = (ViewDataDictionary)HttpContext.Session["b2c_settings"];
            ViewBag.SuccessMessage = HttpContext.Session["saved"];
            HttpContext.Session.Remove("saved");
            return View();
        }

        public ActionResult Error(string message)
        {
            ViewBag.Message = message;

            return View("Error");
        }
    }
}