using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.Repositories;
using DigitalLibrary.Domain.Entities;
using DigitalLibrary.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
namespace DigitalLibrary.Infrastructure.Persistence
{
    public class LibraryDbContext : DbContext, IUnitOfWork
    {
        public LibraryDbContext(DbContextOptions options) : base(options)
        {
        }

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