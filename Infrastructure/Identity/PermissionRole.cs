using System.Collections.Generic;

namespace Infrastructure.Identity
{
    public class PermissionRole
    {
        public int Id { get; set; }

        public ICollection<Permission> Permissions { get; set; } = new HashSet<Permission>();

        public ICollection<Role> Roles { get; set; } = new HashSet<Role>();
    }
}
