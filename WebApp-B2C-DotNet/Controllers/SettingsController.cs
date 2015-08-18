using Microsoft.Owin.Security.Cookies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp_B2C_DotNet.Controllers
{
    public class SettingsController : Controller
    {
        // GET: Settings
        public ActionResult Index()
        {
            ViewData = (ViewDataDictionary) HttpContext.Session["b2c_settings"];
            ViewBag.SuccessMessage = HttpContext.Session["saved"];
            HttpContext.Session.Remove("saved");
            return View();
        }

        [HttpPost]
        public ActionResult Save(string tenant, string client_id, string sign_in_policy, string sign_up_policy, string edit_profile_policy)
        {
            ViewDataDictionary dict = (ViewDataDictionary)HttpContext.Session["b2c_settings"];
            if (dict != null)
            {
                if (dict["tenant"] != null && dict["tenant"] != tenant)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);      
                }
            }


            ViewDataDictionary settings = new ViewDataDictionary
            {
                {"tenant", tenant},
                {"client_id", client_id},
                {"sign_in_policy", sign_in_policy},
                {"sign_up_policy", sign_up_policy},
                {"edit_profile_policy", edit_profile_policy},
            };
            HttpContext.Session.Add("saved", true);
            HttpContext.Session.Add("b2c_settings", settings);
            return new RedirectResult("/Settings");
        }
    }
}