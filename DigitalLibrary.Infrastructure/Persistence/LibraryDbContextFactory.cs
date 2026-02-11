using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DigitalLibrary.Infrastructure.Persistence;

public class LibraryDbContextFactory : IDesignTimeDbContextFactory<LibraryDbContext>
{
    public LibraryDbContext CreateDbContext(string[] args)
    {
        var connectionString = GetConnectionString();

        var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new LibraryDbContext(optionsBuilder.Options);
    }

    private static string GetConnectionString()
    {
        // Try multiple locations - works regardless of where dotnet ef is run from
        var possiblePaths = new[]
        {
            // Run from solution root
            Path.Combine(Directory.GetCurrentDirectory(), "DigitalLibrary.Console", "appsettings.json"),
            // Run from Infrastructure project folder
            Path.Combine(Directory.GetCurrentDirectory(), "../DigitalLibrary.Console/appsettings.json"),
            // Run from Console project folder
            Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"),
        };

        var settingsPath = possiblePaths.FirstOrDefault(File.Exists)
            ?? throw new FileNotFoundException(
                $"appsettings.json not found. Searched:\n{string.Join("\n", possiblePaths)}"
            );

        Console.WriteLine($"[DesignTime] Using settings from: {settingsPath}");

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(settingsPath, optional: false)
            .Build();

        return configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("DefaultConnection string not found");
    }
}