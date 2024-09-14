using devRootsApi.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace devRootsApi.Data
{
    public class DevRootContext:IdentityDbContext<User>
    {
        public DevRootContext (DbContextOptions<DevRootContext> options) : base(options) { }

        public DbSet<Blog>? Blogs { get; set; }
        public DbSet<Category>? Categories { get; set; }
        public DbSet<Comment>? Comments { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            // Seed data
            List<IdentityRole> roles = new()
            {
                new IdentityRole{ Name = "User", NormalizedName = "USER" },
                new IdentityRole{ Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole{ Name = "Blogger", NormalizedName = "BLOGGER" },
            };

            modelBuilder.Entity<IdentityRole>().HasData(roles);

            base.OnModelCreating(modelBuilder);
        }
    }
}