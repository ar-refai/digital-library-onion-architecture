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
    public class BorrowRecordConfiguration : IEntityTypeConfiguration<BorrowRecord>
    {
        public void Configure(EntityTypeBuilder<BorrowRecord> builder)
        {
            // Table
            builder.ToTable("BorrowRecords");
            // PK
            builder.HasKey(br => br.Id);
            builder.Property(br => br.Id).HasConversion(
                id => id.Value,
                value => BorrowRecordId.From(value)
                ).HasField("_id");
            // BorrowerId
            builder.Property(br => br.BorrowerId).HasField("_borrwerId").IsRequired();
            // BorrowedAt
            builder.Property(br => br.BorrowedAt).HasField("_borrowedAt").IsRequired();
            // ReturnedAt
            builder.Property(br => br.ReturnedAt).HasField("_returnedAt").IsRequired(false);
            // BookId
            builder.Property(br => br.BookId).HasConversion(
                id => id.Value,
                value => BookId.From(value)
                ).HasField("_bookId");
            // BookNavigation 
            builder.Navigation(br => br.Book).HasField("_book");
            // index for fast lookup
            builder.HasIndex(br => new { br.BookId, br.ReturnedAt });

        }
    }
}
