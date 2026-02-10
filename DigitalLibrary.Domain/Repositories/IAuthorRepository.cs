using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.Entities;
using DigitalLibrary.Domain.ValueObjects;

namespace DigitalLibrary.Domain.Repositories
{
    public interface IAuthorRepository
    {
        // Query Operations
        Task<Author?> GetByIdAsync(AuthorId id, CancellationToken cancellationToken = default);
        Task<Author?> GetByNameAsynce(string name, CancellationToken cancellationToken = default);
        Task<IReadOnlyCollection<Author>> GetAll(CancellationToken cancellationToken = default);

        // Command Operations
        Task AddAuthorAsync(Author author, CancellationToken cancellationToken = default);
        Task Remove(Author author);
    }
}
