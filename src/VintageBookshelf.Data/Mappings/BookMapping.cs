using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Data.Mappings
{
    public class BookMapping : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.HasKey(b => b.Id);

            builder.Property(b => b.Title)
                .IsRequired()
                .HasColumnType("VARCHAR(200)");
            
            builder.Property(b => b.Publisher)
                .IsRequired()
                .HasColumnType("VARCHAR(200)");
            
            builder.Property(b => b.Summary)
                .IsRequired()
                .HasColumnType("VARCHAR(1000)");

            builder.Property(b => b.ReleaseYear)
                .IsRequired();

            builder.ToTable("Book");
        }
    }
}