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
        public static string defaultTenant = "fabrikamb2c.onmicrosoft.com";
        public static string defaultClientId = "79467a70-1adc-41a2-9d0a-faebefb5866c";
        public static string defaultSignInPolicy = "b2c_1_sign_in";
        public static string defaultSignUpPolicy = "b2c_1_sign_up";
        public static string defaultEditProfilePolicy = "b2c_1_edit_profile";

        // GET: Settings
        public ActionResult Index()
        {
            ViewData = (ViewDataDictionary)HttpContext.Session["b2c_settings"];
            return View();
        }

        [HttpPost]
        public ActionResult Save(string tenant, string client_id, string sign_in_policy, string sign_up_policy, string edit_profile_policy)
        {
            ViewDataDictionary dict = (ViewDataDictionary)HttpContext.Session["b2c_settings"];
            if (dict != null)
            {
                if (dict["tenant"] != null && (string)dict["tenant"] != tenant)
                {
                    HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
                }
            }
            else if (Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
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
            return new RedirectResult("/");
        }
    }
}