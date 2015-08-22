using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp_B2C_DotNet.Utils
{
    public static class ClaimTips
    {
        public static readonly Dictionary<string, string> Descriptions = new Dictionary<string, string>
        {
            {"exp", "Provided in epoch time.  Should be used for checking token validity."},
            {"nbf", "Provided in epoch time.  Should be used for checking token validity."},
            {"ver", "Should always be 1.0."},
            {"iss", "For AAD B2C, takes the form 'https://login.microsoftonline.com/<tenant-guid>/'.  Should be used for validating the token came from a trusted STS."},
            {"acr", "For AAD B2C, it will be the name of the policy used to process the request.  Can be used for ensuring a particular policy was used."},
            {"sub", "A immutable, unique identifier of the user for your app.  Should be used for enforcing access control in your app."},
            {"aud", "For id_tokens, this value will be your app's App Id.  For access_tokens, this value will be the resource identifier, or App ID URI."},
            {"iat", "Provided in epoch time.  Usually the same as the nbf claim value."},
            {"auth_time", "Provided in epoch time.  Can be used for checking when the user last entered their credentials."},
            {"http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Will represent the domain of the idp that signed in the user."},
            {"newUser", "Will always be 'true' if the claim is present in the token."},
            {"name", "Will always be returned in AAD B2C tokens, but will be the empty string if a valid name is not available."},
        };

        public static readonly Dictionary<string, string> Names = new Dictionary<string, string>
        {
            {"exp", "Expiration Time: The expiration time of the token."},
            {"nbf", "Not Before: The time at which the token became valid."},
            {"ver", "Version: The version of the token, as defined by Azure AD."},
            {"iss", "Issuer: The security token service (STS) that emitted the token."},
            {"acr", "Authentication Context Class Reference: Indicates how the subject of the token (the user) was authenticated."},
            {"sub", "Subject: The principal (user) about which the token asserts information."},
            {"aud", "Audience: The resource server that the token was intended for, and that should receieve the token."},
            {"iat", "Issued At: The time at which Azure AD issued the token."},
            {"auth_time", "Authentication Time: The time at which the user last entered their credentials and was authenticated."},
            {"http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "Identity Provider: The service that authenticated the subject (user) and provided claims information."},
            {"newUser", "New User: Indicates if the user who is signing-in was just created in the B2C directory via a sign-up flow."},
            {"name", "Name: A human readable name for the user."},
        };
    }
}
