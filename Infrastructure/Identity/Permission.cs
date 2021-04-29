using System.Collections.Generic;

namespace Infrastructure.Identity
{
    public class Permission
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Description { get; set; }

        public ICollection<PermissionRole> Roles { get; set; } = new HashSet<PermissionRole>();
    }
}
