using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace devRootsApi.Seeds
{
    public static class RoleSeed
    {
        public static void Seed (ModelBuilder modelBuilder)
        {
            List<IdentityRole> roles =
            [
                new IdentityRole { Id = "1", Name = "User", NormalizedName = "USER" },
                new IdentityRole { Id = "2", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "3", Name = "Blogger", NormalizedName = "BLOGGER" }
            ];

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}
