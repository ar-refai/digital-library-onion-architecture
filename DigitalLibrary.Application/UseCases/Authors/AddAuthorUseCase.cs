using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.Repositories;
using DigitalLibrary.Domain.Entities;
using DigitalLibrary.Domain.ValueObjects;
using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations;

namespace DigitalLibrary.Application.UseCases.Authors
{
    public class AddAuthorUseCase
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AddAuthorUseCase(IAuthorRepository authorRepository, IUnitOfWork unitOfWork)
        {
            _authorRepository = authorRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> ExecuteAsync(string name, CancellationToken cancellationToken = default)
        {
            // check for duplicate names
            var existing = await _authorRepository.GetByNameAsynce(name,cancellationToken);
            if (existing != null) throw new InvalidOperationException($"An author with the same name {name} exists");
            // create new author
            var author = new Author(AuthorId.New(), name);
            // excute
            await _authorRepository.AddAuthorAsync(author, cancellationToken);
            // save
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            // return 
            return author.Id.Value;
        }
    }
}
