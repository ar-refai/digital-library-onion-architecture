using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.ValueObjects;

namespace DigitalLibrary.Domain.Entities
{
    public class BorrowRecord
    {
        // Backing Fields
        private BorrowRecordId _id;
        private Guid _borrowerId;
        private DateTime _borrowedAt;
        private DateTime? _returnedAt;
        private Book _book = null;
        private BookId _bookId;

        // EF Core constructor
        private BorrowRecord() { }

        // Domain Constructor
        internal BorrowRecord(BorrowRecordId id, Guid borrowerId, DateTime borrowedAt)
        {
            this._id = id;
            this._borrowerId = borrowerId;
            this._borrowedAt = borrowedAt;
            this._returnedAt = null;
        }

        // Readonly Properties
        public BorrowRecordId Id => _id;
        public Guid BorrowerId => _borrowerId;
        public DateTime BorrowedAt => _borrowedAt;
        public DateTime? ReturnedAt => _returnedAt;
        public BookId BookId => _bookId;
        public Book Book => _book;


        // Behavior

        internal void MarkAsReturned(DateTime returnedAt)
        {
            if (_returnedAt != null)
                throw new ArgumentException("Book is already returned!");
            if (returnedAt < _borrowedAt)
                throw new ArgumentException("Date/Time is not right.");

            _returnedAt = returnedAt;
        }

    }
}
