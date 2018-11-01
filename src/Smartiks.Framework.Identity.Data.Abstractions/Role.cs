using System;
using Microsoft.AspNetCore.Identity;

namespace Smartiks.Framework.Identity.Data.Abstractions
{
    public class Role<TId> : IdentityRole<TId>
        where TId : IEquatable<TId>
    {
    }
}
