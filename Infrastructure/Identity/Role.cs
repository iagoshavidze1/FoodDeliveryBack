using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Infrastructure.Identity
{
    public class Role : IdentityRole<string>
    {
        public ICollection<PermissionRole> Permissions { get; set; }
    }
}
