using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.Repositories;
using DigitalLibrary.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using DigitalLibrary.Infrastructure.Persistence.Configurations;
namespace DigitalLibrary.Infrastructure.Persistence
{
    public class LibraryDbContext : DbContext, IUnitOfWork
    {
        public DbSet<Book> Books => Set<Book>();
        public DbSet<Author> Authors => Set<Author>();
        public DbSet<BorrowRecord> BorrowRecords => Set<BorrowRecord>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new BorrowRecordConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
