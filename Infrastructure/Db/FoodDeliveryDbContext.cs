using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Db
{
    public class FoodDeliveryDbContext : IdentityDbContext<User,Role,string>
    {
        public FoodDeliveryDbContext(DbContextOptions<FoodDeliveryDbContext> options):base(options)
        {

        }


        public override DbSet<User> Users { get; set; }
        public override DbSet<Role> Roles { get; set; }

        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }


    }
}
