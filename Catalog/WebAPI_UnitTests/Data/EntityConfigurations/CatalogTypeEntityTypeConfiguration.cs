using WebAPI_UnitTests.Data.Entities;

namespace WebAPI_UnitTests.Data.EntityConfigurations;

public class CatalogTypeEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogTypeEntity>
{
    public void Configure(EntityTypeBuilder<CatalogTypeEntity> builder)
    {
        builder.ToTable("CatalogType");

        builder.HasKey(ci => ci.Id);

        builder.Property(ci => ci.Id)
            .UseHiLo("catalog_type_hilo")
            .IsRequired();

        builder.Property(cb => cb.Type)
            .IsRequired()
            .HasMaxLength(100);
    }
}