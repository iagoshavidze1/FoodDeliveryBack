using System.Collections.Generic;

namespace Infrastructure.Identity
{
    public class PermissionRole
    {
        public int Id { get; set; }

        public Permission Permission { get; set; }

        public Role Role { get; set; }
    }
}
