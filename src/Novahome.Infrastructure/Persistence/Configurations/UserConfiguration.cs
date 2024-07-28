namespace Novahome.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
  public void Configure(EntityTypeBuilder<User> builder)
  {
    builder.Property(p => p.GivenName)
      .HasMaxLength(250)
      .IsRequired();

    builder.Property(p => p.FamilyName)
      .HasMaxLength(250)
      .IsRequired();

    builder.Property(p => p.EmailAddress)
      .HasMaxLength(250)
      .IsRequired();
  }
}
