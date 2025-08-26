using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurants.Infrastructure.Persistence
{
    public class AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : IdentityDbContext(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var readerRoleId = "c3d28f20-52fc-4985-bf4e-fa8b52efb7f6";
            var writerRoleId = "39e7c27b-e71c-4302-9fb8-a930c9ec58af";
            var roles = new List<IdentityRole>
            {
                new IdentityRole{
                Id=readerRoleId,
                ConcurrencyStamp=readerRoleId,
                Name="Reader",
                NormalizedName="Reader".ToUpper()
                },
                new IdentityRole{
                Id=writerRoleId,
                ConcurrencyStamp=writerRoleId,
                Name="Writer",
                NormalizedName="Writer".ToUpper()
                }

            };
            builder.Entity<IdentityRole>().HasData(roles);
        }

    }
}
