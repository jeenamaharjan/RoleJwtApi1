using Microsoft.EntityFrameworkCore;
using RoleJwtApi1.Models;

namespace RoleJwtApi1.Context
{
    public class JwtContext : DbContext
    {
        // Constructor to pass DbContextOptions
        public JwtContext(DbContextOptions<JwtContext> options) : base(options)
        {
        }

        // Define DbSet properties for each model
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Employee> Employees { get; set; }
    }
}
