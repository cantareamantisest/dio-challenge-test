using Desafio.Dio.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Desafio.Dio.Repository.Mappings
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.Name)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(t => t.Price)
                .HasColumnType("decimal(18, 2)")
                .IsRequired();

            builder.HasOne(t => t.Category)
                .WithMany(t => t.Products)
                .HasForeignKey(t => t.IdCategory)
                .IsRequired();
        }
    }
}
