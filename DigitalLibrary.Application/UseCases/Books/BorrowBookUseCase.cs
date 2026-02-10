using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.Repositories;
using DigitalLibrary.Domain.ValueObjects;

namespace DigitalLibrary.Application.UseCases.Books
{
    public class BorrowBookUseCase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookRepository _bookRepository;

        public BorrowBookUseCase(IUnitOfWork unitOfWork, IBookRepository bookRepository)
        {
            _unitOfWork = unitOfWork;
            _bookRepository = bookRepository;
        }

        public async Task ExcuteAsync(Guid id, Guid borrowerId, CancellationToken cancellationToken = default)
        {
            // 1. fetch the aggregate
            var book = await _bookRepository.GetByIdAsync(BookId.From(id),cancellationToken);
            if (book == null) throw new InvalidOperationException($"The book with Id {id}does not exist");
            // 2. excute
            book.Borrow(borrowerId);
            // 3. save -> persist
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }




    }
}
