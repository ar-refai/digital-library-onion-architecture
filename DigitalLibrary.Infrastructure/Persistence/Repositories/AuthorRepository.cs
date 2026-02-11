using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.Entities;
using DigitalLibrary.Domain.Repositories;
using DigitalLibrary.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace DigitalLibrary.Infrastructure.Persistence.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly LibraryDbContext _context;
        public AuthorRepository(LibraryDbContext context) {
            _context = context;
        }

        public async Task<Author?> GetByIdAsync(AuthorId id, CancellationToken cancellationToken = default)
        {
            return await _context.Authors.Include(a=>a.Books).FirstOrDefaultAsync(a =>a.Id == id, cancellationToken);
        }

        public async Task<Author?> GetByNameAsynce(string name, CancellationToken cancellationToken = default)
        {
            return await _context.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Name == name, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Author>> GetAll(CancellationToken cancellationToken = default)
        {
            return await _context.Authors.AsNoTracking().OrderBy(a=>a.Name).ToListAsync(cancellationToken);
            
        }

        public async Task AddAuthorAsync(Author author, CancellationToken cancellationToken = default)
        {
            await _context.Authors.AddAsync(author,cancellationToken);
        }

        public void Remove(Author author)
        {
            _context.Authors.Remove(author);
        }
    }
}
