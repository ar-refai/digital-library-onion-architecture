using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.ValueObjects;

namespace DigitalLibrary.Domain.Entities
{
    public class Book
    {
        // Backing Fields
        private BookId _id;
        private string _title = null;
        private string _isbn = null;
        private AuthorId _authorId;
        private Author _author;
        private List<BorrowRecord> _borrowRecords = new();

        // public readonly properties no setters
        public BookId Id => _id;
        public string Title => _title;
        public string Isbn => _isbn;
        public AuthorId AuthorId => _authorId;
        public Author Author => _author;
        public IReadOnlyCollection<BorrowRecord> BorrowRecords => _borrowRecords.AsReadOnly();

        // Private Constructor for EF Core
        private Book() { }

        // Domain Constructor 
        public Book (BookId id, string title, string isbn, Author author)
        {
            if (string.IsNullOrEmpty(title)) throw new ArgumentException("");
            if (string.IsNullOrEmpty(isbn)) throw new ArgumentException("");
            ArgumentNullException.ThrowIfNull(author);
            _id = id;
            _title = title;
            _isbn = isbn;
            _authorId = author.Id;
            _author = author;
        }

        // Computed Availability
        public bool IsAvailable() => !_borrowRecords.Any(b=> b.ReturnedAt == null);

        // Behavior - Borrow() - Return()
        public void Borrow(Guid borrowerId)
        {
            if (!IsAvailable()) throw new Exception("The book is not currently available.");
            var record = new BorrowRecord(BorrowRecordId.New(), borrowerId ,DateTime.UtcNow );
            _borrowRecords.Add(record);
        }

        public void Return()
        {
            if (IsAvailable()) throw new Exception("The book is not currently borrowed.");
            var activeRecord = _borrowRecords.SingleOrDefault(br => br.ReturnedAt == null);
            activeRecord!.MarkAsReturned(DateTime.UtcNow);  // acotding to logic can't get to here if no null returned at records
        }

    }
}
