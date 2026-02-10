using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.Repositories;
using DigitalLibrary.Domain.Entities;
using DigitalLibrary.Domain.ValueObjects;
using DigitalLibrary.Domain;



namespace DigitalLibrary.Application.UseCases.Books
{
    public class AddBookUseCase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AddBookUseCase(IBookRepository bookRepository, IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _authorRepository = authorRepository;
            _bookRepository = bookRepository;
        }
        public async Task<Guid> ExcuteAsync (string title, string isbn, Guid authorId, CancellationToken cancellationToken = default)
        {
            // 1. fetch the author
            var author = await _authorRepository.GetByIdAsync(AuthorId.From(authorId), cancellationToken);
            if (author == null) throw new InvalidOperationException($"The Author with ID {authorId} does not exist");

            // 2. check for duplicate ISBN
            var book = await _bookRepository.GetByIsbnAsync(isbn, cancellationToken);
            if (!(book == null)) throw new InvalidOperationException($"The Book with ISBN {isbn} already exists.");

            // 3. create book object
            var newBook = new Book(BookId.New(),title, isbn, author) ;

            // 2. excute domain logic
            await _bookRepository.AddAsync(newBook);

            // 3. save changes
            await _unitOfWork.SaveChangesAsync();

            return newBook.Id.Value;
        }

    }
}
