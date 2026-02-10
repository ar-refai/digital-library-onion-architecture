using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DigitalLibrary.Domain.Entities;
using DigitalLibrary.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalLibrary.Infrastructure.Persistence.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            // Table
            builder.ToTable("Books");

            // Primary key
            builder.HasKey(b => b.Id);
            
            // Id Conversion
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).HasConversion(
                id => id.Value,
                value => BookId.From(value)
                ).HasField("_id");

            // Title
            builder.Property(b => b.Title)
                .HasField("_title")
                .HasMaxLength(200)
                .IsRequired();

            // ISBN
            builder.Property(b => b.Isbn)
                .HasField("_isbn")
                .HasMaxLength(20)
                .IsRequired();

            // Author Id Conversion
            builder.Property(b => b.AuthorId)
                .HasConversion(
                    id => id.Value,
                    value => AuthorId.From(value)
                ).HasField("_authorId");

            // Author Navigation Property
            builder.Navigation(b => b.Author).HasField("_author");

            // BorrowRecords - private collection via backing field
            builder.HasMany(b => b.BorrowRecords)
                .WithOne(br => br.Book)
                .HasForeignKey(br => br.BookId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(b => b.BorrowRecords).HasField("_borrowRecords").UsePropertyAccessMode(PropertyAccessMode.Field);

            // Indexing the title
            builder.HasIndex(b => b.Isbn).IsUnique();
        }

    }
}
