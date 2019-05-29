using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace zSpec.Tests.Context
{
    /// <summary>
    /// Entity configuration of <see cref="User" />
    /// </summary>
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(agent => agent.Name).IsRequired();
            builder.Property(agent => agent.Age).IsRequired();
            builder.HasKey(agent => agent.Id);
        }
    }
}