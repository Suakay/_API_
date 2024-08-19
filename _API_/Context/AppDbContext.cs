using _21_MVC_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace _21_MVC_API.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions options):base(options) 
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole()
                {

                    Name = "User",
                    NormalizedName = "USER",
                },
                  new IdentityRole()
                  {
                      Name = "Admin",
                      NormalizedName = "ADMİN",
                  }
                  );

            base.OnModelCreating(builder);
        }
    }
}
