using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VintageBookshelf.Domain.Models;

namespace VintageBookshelf.Data.Mappings
{
    public class AuthorMapping : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Name)
                .IsRequired()
                .HasColumnType("VARCHAR(200)");
            
            builder.Property(a => a.Biography)
                .IsRequired()
                .HasColumnType("VARCHAR(1000)");
            
            builder.Property(a => a.BirthDate)
                .IsRequired()
                .HasColumnType("DATETIMEOFFSET");

            builder.HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId);

            builder.ToTable("Author");
        }
    }
}