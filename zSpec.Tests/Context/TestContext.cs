using Microsoft.EntityFrameworkCore;

namespace zSpec.Tests.Context
{
    public class TestContext : DbContext
    {
        public TestContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}