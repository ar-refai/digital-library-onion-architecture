using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.Entities;
using DigitalLibrary.Domain.Repositories;
using DigitalLibrary.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.Infrastructure.Persistence.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryDbContext _context;
        public BookRepository(LibraryDbContext context)
        {
            _context = context;
        }


        public async Task<Book?> GetByIdAsync(BookId bookId, CancellationToken cancellationToken = default)
        {
            return await _context.Books
                .Include(b => b.BorrowRecords)
                .OrderBy(b => b.Title)
                .FirstOrDefaultAsync(b => b.Id == bookId, cancellationToken);
        }


        public async Task<Book?> GetByIsbnAsync(string isbn, CancellationToken cancellationToken = default)
        {
            return await _context.Books
                .AsNoTracking()
                .OrderBy(b=>b.Title)
                .FirstOrDefaultAsync(b=>b.Isbn == isbn, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Book>> GetAvailableBooksAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Books
                .AsNoTracking()
                .Include(b => b.Author)
                .Include(b => b.BorrowRecords)
                .Where(b => b.BorrowRecords
                .All(br => br.ReturnedAt != null))
                .OrderBy(b=>b.Title)
                .ToListAsync(cancellationToken);
        }

        public async Task<IReadOnlyCollection<Book>> GetByAuthorAsync(AuthorId authorId, CancellationToken cancellationToken = default)
        {
            return await _context.Books
                .AsNoTracking()
                .Include(b => b.BorrowRecords)
                .Where(b => b.AuthorId == authorId)
                .OrderBy(b => b.Title)
                .ToListAsync(cancellationToken);
        }

        public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
        {
            await _context.Books.AddAsync(book, cancellationToken);
        }

        public void Remove(Book book)
        {
            _context.Books.Remove(book);
        }
    }
}
