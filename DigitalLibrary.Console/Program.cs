using DigitalLibrary.Application;
using DigitalLibrary.Application.UseCases.Authors;
using DigitalLibrary.Application.UseCases.Books;
using DigitalLibrary.Infrastructure;
using DigitalLibrary.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ─── 1. Build Configuration ───────────────────────────────────────────────────

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

var connectionString = configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string not found");

// ─── 2. Register Services (Composition Root) ──────────────────────────────────

var services = new ServiceCollection();

services.AddApplication();
services.AddInfrastructure(connectionString);

var serviceProvider = services.BuildServiceProvider();

// ─── 3. Apply Migrations ───────────────────────────────────────────────────────

using (var scope = serviceProvider.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
    await db.Database.MigrateAsync();
    Console.WriteLine("✅ Database migrated successfully");
}

// ─── 4. Demo Scenarios ────────────────────────────────────────────────────────

using (var scope = serviceProvider.CreateScope())
{
    var addAuthor = scope.ServiceProvider.GetRequiredService<AddAuthorUseCase>();
    var addBook = scope.ServiceProvider.GetRequiredService<AddBookUseCase>();
    var borrowBook = scope.ServiceProvider.GetRequiredService<BorrowBookUseCase>();
    var returnBook = scope.ServiceProvider.GetRequiredService<ReturnBookUseCase>();

    try
    {
        // ── Scenario A: Add Author ─────────────────────────────────────────────
        Console.WriteLine("\n── Scenario A: Add Author ──");
        var authorId = await addAuthor.ExecuteAsync("Eric Evans");
        Console.WriteLine($"✅ Author created: {authorId}");

        // ── Scenario B: Add Book ───────────────────────────────────────────────
        Console.WriteLine("\n── Scenario B: Add Book ──");
        var bookId = await addBook.ExecuteAsync(
            "Domain-Driven Design",
            "978-0321125217",
            authorId
        );
        Console.WriteLine($"✅ Book created: {bookId}");

        // ── Scenario C: Borrow Book ────────────────────────────────────────────
        Console.WriteLine("\n── Scenario C: Borrow Book ──");
        var borrowerId = Guid.NewGuid();
        await borrowBook.ExecuteAsync(bookId, borrowerId);
        Console.WriteLine($"✅ Book borrowed by: {borrowerId}");

        // ── Scenario D: Borrow Already Borrowed Book ───────────────────────────
        Console.WriteLine("\n── Scenario D: Borrow Already Borrowed Book ──");
        try
        {
            await borrowBook.ExecuteAsync(bookId, Guid.NewGuid());
            Console.WriteLine("❌ Should have thrown exception!");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"✅ Correctly rejected: {ex.Message}");
        }

        // ── Scenario E: Return Book ────────────────────────────────────────────
        Console.WriteLine("\n── Scenario E: Return Book ──");
        await returnBook.ExecuteAsync(bookId);
        Console.WriteLine("✅ Book returned successfully");

        // ── Scenario F: Borrow Again After Return ──────────────────────────────
        Console.WriteLine("\n── Scenario F: Borrow Again After Return ──");
        await borrowBook.ExecuteAsync(bookId, Guid.NewGuid());
        Console.WriteLine("✅ Book borrowed again after return");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Unexpected error: {ex.Message}");
        Console.WriteLine(ex.StackTrace);
    }
}

Console.WriteLine("\n✅ All scenarios complete");