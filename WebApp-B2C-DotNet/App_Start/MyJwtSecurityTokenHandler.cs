using Microsoft.IdentityModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace WebApp_B2C_DotNet.App_Start
{
    class MyJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        public MyJwtSecurityTokenHandler() : base() {
            InboundClaimTypeMap = new Dictionary<string, string>();
        }
    }
}
