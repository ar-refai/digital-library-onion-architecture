using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigitalLibrary.Domain.Repositories;
using DigitalLibrary.Infrastructure.Persistence;
using DigitalLibrary.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DigitalLibrary.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services, 
            string connectionString)
        {
            // register dbcontext
            services.AddDbContext<LibraryDbContext>(options => options
            .UseSqlServer(connectionString)
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            );

            // register repositories
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IBookRepository,BookRepository>();

            // register unit of work
            services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<LibraryDbContext>());
            
            return services;
        }
      
    }
}
