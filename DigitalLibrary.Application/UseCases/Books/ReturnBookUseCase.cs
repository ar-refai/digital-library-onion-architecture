using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.Repositories;
using DigitalLibrary.Domain.ValueObjects;

namespace DigitalLibrary.Application.UseCases.Books
{
    public class ReturnBookUseCase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReturnBookUseCase(IBookRepository bookRepository, IUnitOfWork unitOfWork)
        {
            _bookRepository = bookRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ExecuteAsync(Guid bookId, CancellationToken cancellationToken = default)
        {
            // 1. fetch the aggregation (book)
            var book = await _bookRepository.GetByIdAsync(BookId.From(bookId), cancellationToken);
            if (book == null) throw new InvalidOperationException($"The book with ID {bookId} does not exist ");
            // 2. excute domain logic
            book.Return();
            // 3. save changes -> (persist)
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
