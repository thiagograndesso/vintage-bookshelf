using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Data.Mappings
{
    public class BookshelfMapping : IEntityTypeConfiguration<Bookshelf>
    {
        public void Configure(EntityTypeBuilder<Bookshelf> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(200)");
            
            builder.Property(b => b.City)
                .IsRequired()
                .HasColumnType("VARCHAR(200)");
            
            builder.Property(b => b.Address)
                .IsRequired()
                .HasColumnType("VARCHAR(200)");
            
            builder.HasMany(b => b.Books)
                .WithOne(b => b.Bookshelf)
                .HasForeignKey(b => b.BookshelfId);

            builder.ToTable("Bookshelf");
        }
    }
}