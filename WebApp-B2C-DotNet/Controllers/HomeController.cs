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
            ViewDataDictionary dict = (ViewDataDictionary)HttpContext.Session["b2c_settings"];
            if (dict == null) 
            {
                dict = new ViewDataDictionary
                {
                    {"tenant", SettingsController.defaultTenant},
                    {"client_id", SettingsController.defaultClientId},
                    {"sign_in_policy", SettingsController.defaultSignInPolicy},
                    {"sign_up_policy", SettingsController.defaultSignUpPolicy},
                    {"edit_profile_policy", SettingsController.defaultEditProfilePolicy},
                };
            }

            ViewData = dict;
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