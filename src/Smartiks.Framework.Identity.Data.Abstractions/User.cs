using System;
using Microsoft.AspNetCore.Identity;

namespace Smartiks.Framework.Identity.Data.Abstractions
{
    public class User<TId> : IdentityUser<TId>
        where TId : IEquatable<TId>
    {
    }
}
