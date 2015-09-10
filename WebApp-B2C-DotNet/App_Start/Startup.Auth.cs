using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// The following using statements were added for this sample
using Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Notifications;
using Microsoft.IdentityModel.Protocols;
using System.Web.Mvc;
using System.Configuration;
using System.IdentityModel.Tokens;
using WebApp_B2C_DotNet.App_Start;

namespace WebApp_B2C_DotNet
{
	public partial class Startup
	{
        private const string discoverySuffix = "/.well-known/openid-configuration";
        public const string AcrClaimType = "acr";
        public const string IssuerClaimType = "iss";
        public const string AudClaimType = "aud";
        public const string NewUserClaimType = "newUser";

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            // Bootstrap with my tenant's config
            OpenIdConnectAuthenticationOptions options = new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://login.microsoftonline.com/strockisdevthree.onmicrosoft.com",
                ClientId = "ec5465e6-f48e-4ec2-a76a-ea99891a8d84",
                RedirectUri = "https://aadb2cplayground.azurewebsites.net/",
                PostLogoutRedirectUri = "https://aadb2cplayground.azurewebsites.net/",
                //RedirectUri = "https://localhost:44316/",
                //PostLogoutRedirectUri = "https://localhost:44316/",
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = OnAuthenticationFailed,
                    RedirectToIdentityProvider = OnRedirectToIdentityProvider,
                    SecurityTokenValidated = OnSecurityTokenValidated,
                },

                Scope = "openid",
                ResponseType = "id_token",
                ConfigurationManager = new B2CConfigurationManager("https://login.microsoftonline.com/strockisdevthree.onmicrosoft.com/v2.0/.well-known/openid-configuration"),
                SecurityTokenHandlers = new SecurityTokenHandlerCollection(new List<SecurityTokenHandler> { new MyJwtSecurityTokenHandler() }),

                TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = "name",
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = false,
                    ValidateAudience = false,
                },
                ProtocolValidator = new OpenIdConnectProtocolValidator 
                { 
                    RequireNonce = false,
                },
            };

            app.RequireAspNetSession();

            app.Use(typeof(B2COpenIdConnectAuthenticationMiddleware), app, options);
                
        }

        private async Task OnSecurityTokenValidated(SecurityTokenValidatedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            if (string.IsNullOrEmpty(notification.ProtocolMessage.State)) 
            {
                string tenant = notification.AuthenticationTicket.Identity.FindFirst(IssuerClaimType).Value.Split('/')[3];
                ViewDataDictionary settings = new ViewDataDictionary
                {
                    {"tenant", tenant},
                    {"client_id", notification.AuthenticationTicket.Identity.FindFirst(AudClaimType).Value},
                    {"sign_up_policy", notification.AuthenticationTicket.Identity.HasClaim(NewUserClaimType, "true") ? notification.AuthenticationTicket.Identity.FindFirst(AcrClaimType).Value : null},
                    {"sign_in_policy", null},
                    {"edit_profile_policy", null},
                };
                HttpContext.Current.Session.Add("b2c_settings", settings);
            }
        }

        private async Task OnRedirectToIdentityProvider(RedirectToIdentityProviderNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            string[] tenant = new string[1], client_id = new string[1], policy = new string[1];
            if (!notification.Response.Headers.TryGetValue("tenant", out tenant) ||
                !notification.Response.Headers.TryGetValue("client_id", out client_id) ||
                !notification.Response.Headers.TryGetValue("policy", out policy) || 
                string.IsNullOrEmpty(tenant[0]) || string.IsNullOrEmpty(client_id[0]) || string.IsNullOrEmpty(policy[0])) 
            {
                notification.HandleResponse();
                notification.Response.Redirect("/Home/Error?message=You need to input your app settings before you can try this flow.");
                return;
            }

            try
            {
                B2CConfigurationManager cm = new B2CConfigurationManager(String.Format("https://login.microsoftonline.com/{0}/v2.0/.well-known/openid-configuration", tenant[0]));
                OpenIdConnectConfiguration config = await cm.GetConfigurationAsync(new System.Threading.CancellationToken(), policy[0]);
                notification.ProtocolMessage.IssuerAddress = config.AuthorizationEndpoint;
                notification.ProtocolMessage.ClientId = client_id[0];
                notification.ProtocolMessage.SetParameter("prompt", "login");
                notification.Response.Headers.Remove("tenant");
                notification.Response.Headers.Remove("client_id");
                notification.Response.Headers.Remove("policy");
            }
            catch (Exception e)
            {
                notification.HandleResponse();
                notification.Response.Redirect("/Home/Error?message=Are you SURE you entered your app settings correctly?");
            }
            
            return;
        }

        // Used for avoiding yellow-screen-of-death
        private Task OnAuthenticationFailed(AuthenticationFailedNotification<OpenIdConnectMessage, OpenIdConnectAuthenticationOptions> notification)
        {
            notification.HandleResponse();
            notification.Response.Redirect("/Home/Error?message=" + notification.Exception.Message);
            return Task.FromResult(0);
        }
    }
}