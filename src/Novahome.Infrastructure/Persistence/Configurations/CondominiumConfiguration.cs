namespace Novahome.Infrastructure.Persistence.Configurations;

public class CondominiumConfiguration : IEntityTypeConfiguration<Condominium>
{
  public void Configure(EntityTypeBuilder<Condominium> builder)
  {
    builder.Property(p => p.Name)
      .HasMaxLength(250)
      .IsRequired();

    builder.Property(p => p.FiscalCode)
      .HasMaxLength(11)
      .IsRequired();

    builder.HasOne(condominium => condominium.CreatedBy)
      .WithMany(user => user.CreatedCondominiums)
      .HasForeignKey(condominium => condominium.CreatedById);

    builder.HasOne(condominium => condominium.LastModifiedBy)
      .WithMany(user => user.LastModifiedCondominiums)
      .HasForeignKey(condominium => condominium.LastModifiedById);
  }
}
