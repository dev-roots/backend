using devRootsApi.Models;
using devRootsApi.Seeds;
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
            base.OnModelCreating(modelBuilder);
                
            // Seed models
            UserSeed.SeedAdmin(modelBuilder);
        }
    }
}