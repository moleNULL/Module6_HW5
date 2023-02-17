using WebAPI_UnitTests.Data.Entities;

namespace WebAPI_UnitTests.Data.EntityConfigurations;

public class CatalogItemEntityTypeConfiguration
    : IEntityTypeConfiguration<CatalogItemEntity>
{
    public void Configure(EntityTypeBuilder<CatalogItemEntity> builder)
    {
        builder.ToTable("Catalog");

        builder.Property(ci => ci.Id)
            .UseHiLo("catalog_hilo")
            .IsRequired();

        builder.Property(ci => ci.Name)
            .IsRequired(true)
            .HasMaxLength(50);

        builder.Property(ci => ci.Price)
            .IsRequired(true);

        builder.Property(ci => ci.PictureFileName)
            .IsRequired(false);

        builder.HasOne(ci => ci.CatalogBrand)
            .WithMany()
            .HasForeignKey(ci => ci.CatalogBrandId);

        builder.HasOne(ci => ci.CatalogType)
            .WithMany()
            .HasForeignKey(ci => ci.CatalogTypeId);
    }
}