using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.ValueObjects;

namespace DigitalLibrary.Domain.Entities
{
    public class Author
    {
        // Backing fields EF Core will write to them using reflection 
        private AuthorId _id;
        private string _name = null;
        private readonly List<Book> _books = new();

        private Author() { } // for EF Core to set the properties using reflection 
        public Author(AuthorId id, string name)
        {
            if(string.IsNullOrWhiteSpace(name)) 
                throw new ArgumentException("Author name cannot be null or empty.", nameof(name));
            _id = id;
            _name = name;
        }

        // Read-only properties for external access no setters to prevent modification after creation
        public AuthorId Id => _id;
        public string Name => _name;
        public IReadOnlyCollection<Book> Books => _books.AsReadOnly();

        // Behavior
        public void AddBook(Book book)
        {
            if(book is null)
                throw new ArgumentException("Book cannot be null.", nameof(book));
            if (book.AuthorId != Id)
                throw new ArgumentException("Book does not belong to this author.", nameof(book));
            if (_books.Any(b => b.Id == book.Id))
                throw new ArgumentException("Book already exists for this user", nameof(book));
            _books.Add(book);
        }
    }
}
