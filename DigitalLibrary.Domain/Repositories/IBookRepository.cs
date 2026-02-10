using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.Entities;
using DigitalLibrary.Domain.ValueObjects;

namespace DigitalLibrary.Domain.Repositories
{
    public interface IBookRepository
    {
        // Read operations
        Task<Book?> GetByIdAsync(BookId bookId, CancellationToken cancellationToken = default);
        Task<Book?> GetByIsbnAsync(string isbn,CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Book>> GetAvailableBooksAsync(CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Book>> GetByAuthorAsync(AuthorId authorId, CancellationToken cancellationToken = default);

        // Write Operations
        // No Update.. Because you can get the book by get by id, EF Core would track the returned object, and you can update it then.
        Task AddAsync(Book book, CancellationToken cancellationToken = default);
        void Remove(Book book);

    }
}
