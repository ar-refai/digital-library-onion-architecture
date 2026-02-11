using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Application.UseCases.Authors;
using DigitalLibrary.Application.UseCases.Books;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalLibrary.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // register author's usecases
            services.AddScoped<AddAuthorUseCase>();

            // register book's usecases
            services.AddScoped<AddBookUseCase>();
            services.AddScoped<BorrowBookUseCase>();
            services.AddScoped<ReturnBookUseCase>();
            return services;
        }
    }
}
