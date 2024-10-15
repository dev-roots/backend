using devRootsApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace devRootsApi.Seeds
{
    public static class UserSeed
    {
        public static void SeedAdmin (ModelBuilder modelBuilder)
        {
            // Ensure roles are seeded first
            RoleSeed.Seed(modelBuilder);

            // Seed users
            List<User> users = new()
            {
                new User { UserName = "rgomez", Email = "rafagomezguillen03@gmail.com" },
                new User { UserName = "gafonso", Email = "gafonsoudev@gmail.com" },
                new User { UserName = "aizquierdo", Email = "andres.izbri@gmail.com" }
            };

            var passwordHasher = new PasswordHasher<User>();
            string adminRoleId = "2";
            List<IdentityUserRole<string>> userRoles = new();

            foreach (var user in users)
            {
                user.NormalizedUserName = user.UserName.ToUpper();
                user.NormalizedEmail = user.Email.ToUpper();
                user.ProfilePicture = "https://www.gravatar.com/avatar/";
                user.CreatedAt = DateTime.Now;
                user.UpdatedAt = DateTime.Now;
                user.PasswordHash = passwordHasher.HashPassword(user, "Asdf1234!");

                userRoles.Add(new IdentityUserRole<string> { UserId = user.Id, RoleId = adminRoleId });
            }

            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<IdentityUserRole<string>>().HasData(userRoles);
        }
    }
}
