using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Smartiks.Framework.Identity.Server.Abstractions
{
    public class IdentityAuthenticationServerOptions
    {
        public SigningCredentials SigningCredentials { get; set; }

        public Action<DbContextOptionsBuilder> DbContextOptionsBuilder { get; set; }
    }
}
