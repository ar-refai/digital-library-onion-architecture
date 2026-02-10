using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.Entities;
using DigitalLibrary.Domain.ValueObjects;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DigitalLibrary.Infrastructure.Persistence.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            // Table
            builder.ToTable("Authors");

            // Primary Key - value object conversion
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).HasConversion(
                id => id.Value,             // AuthorId -> Guid (to DB)
                value => AuthorId.From(value) // Guid -> AuthorId (from DB)
                ).HasField("_id");

            // Name - backed field
            builder.Property(a => a.Name)
                .HasField("_name")
                .HasMaxLength(200)
                .IsRequired();

            // Books navigation
            builder.HasMany(a => a.Books)
                .WithOne(b => b.Author)
                .HasForeignKey(b => b.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Index to lookup by name
            builder.HasIndex(a => a.Name)
                .IsUnique();


        }
    }
}
