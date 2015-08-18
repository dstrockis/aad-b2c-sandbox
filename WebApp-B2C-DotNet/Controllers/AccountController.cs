using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

// The following using statements were added for this sample.
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Microsoft.Owin.Security.Cookies;
using WebApp_B2C_DotNet.App_Start;
using System.Security.Claims;

namespace WebApp_B2C_DotNet.Controllers
{
    public class AccountController : Controller
    {
        public void SignIn()
        {
            if (!Request.IsAuthenticated)
            {
                ViewDataDictionary dict = (ViewDataDictionary)HttpContext.Session["b2c_settings"];
                if (dict != null)
                {
                    if (dict["tenant"] != null)
                        Response.Headers.Add("tenant", (string)dict["tenant"]);
                    if (dict["client_id"] != null)
                        Response.Headers.Add("client_id", (string)dict["client_id"]);
                    if (dict["sign_in_policy"] != null)
                        Response.Headers.Add("policy", (string)dict["sign_in_policy"]);
                }
                
                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties
                    {
                        RedirectUri = "/",
                    },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }
        public void SignUp()
        {
            if (!Request.IsAuthenticated)
            {
                ViewDataDictionary dict = (ViewDataDictionary)HttpContext.Session["b2c_settings"];
                if (dict != null)
                {
                    if (dict["tenant"] != null)
                        Response.Headers.Add("tenant", (string)dict["tenant"]);
                    if (dict["client_id"] != null)
                        Response.Headers.Add("client_id", (string)dict["client_id"]);
                    if (dict["sign_up_policy"] != null)
                        Response.Headers.Add("policy", (string)dict["sign_up_policy"]);
                }

                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties
                    {
                        RedirectUri = "/",
                    },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }


        public void Profile()
        {
            if (Request.IsAuthenticated)
            {
                ViewDataDictionary dict = (ViewDataDictionary)HttpContext.Session["b2c_settings"];
                if (dict != null)
                {
                    if (dict["tenant"] != null)
                        Response.Headers.Add("tenant", (string)dict["tenant"]);
                    if (dict["client_id"] != null)
                        Response.Headers.Add("client_id", (string)dict["client_id"]);
                    if (dict["edit_profile_policy"] != null)
                        Response.Headers.Add("policy", (string)dict["edit_profile_policy"]);
                }

                HttpContext.GetOwinContext().Authentication.Challenge(
                    new AuthenticationProperties
                    {
                        RedirectUri = "/",
                    },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public ActionResult SignOut()
        {
            HttpContext.GetOwinContext().Authentication.SignOut(CookieAuthenticationDefaults.AuthenticationType);
            return new RedirectResult("/");
        }
	}
}