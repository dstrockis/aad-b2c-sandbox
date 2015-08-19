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
        public const string AcrClaimType = "http://schemas.microsoft.com/claims/authnclassreference";

        public void ConfigureAuth(IAppBuilder app)
        {
            app.SetDefaultSignInAsAuthenticationType(CookieAuthenticationDefaults.AuthenticationType);

            app.UseCookieAuthentication(new CookieAuthenticationOptions());

            // Bootstrap with my tenant's config
            OpenIdConnectAuthenticationOptions options = new OpenIdConnectAuthenticationOptions
            {
                Authority = "https://login.microsoftonline.com/strockisdevthree.onmicrosoft.com",
                ClientId = "ec5465e6-f48e-4ec2-a76a-ea99891a8d84",
                RedirectUri = "http://aadb2csandbox.azurewebsites.net/",
                PostLogoutRedirectUri = "http://aadb2csandbox.azurewebsites.net/",
                Notifications = new OpenIdConnectAuthenticationNotifications
                {
                    AuthenticationFailed = OnAuthenticationFailed,
                    RedirectToIdentityProvider = OnRedirectToIdentityProvider,
                },

                Scope = "openid",
                ConfigurationManager = new B2CConfigurationManager("https://login.microsoftonline.com/strockisdevthree.onmicrosoft.com/.well-known/openid-configuration"),

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

            app.Use(typeof(B2COpenIdConnectAuthenticationMiddleware), app, options);
                
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

            B2CConfigurationManager cm = new B2CConfigurationManager(String.Format("https://login.microsoftonline.com/{0}/.well-known/openid-configuration", tenant[0]));
            OpenIdConnectConfiguration config = await cm.GetConfigurationAsync(new System.Threading.CancellationToken(), policy[0]);
            notification.ProtocolMessage.IssuerAddress = config.AuthorizationEndpoint;
            notification.ProtocolMessage.ClientId = client_id[0];
            notification.ProtocolMessage.SetParameter("prompt", "login");
            notification.Response.Headers.Remove("tenant");
            notification.Response.Headers.Remove("client_id");
            notification.Response.Headers.Remove("policy");
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