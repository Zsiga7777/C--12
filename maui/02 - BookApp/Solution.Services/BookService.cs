using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Solution.Core.Interfaces;
using Solution.Core.Models;
using Solution.Database.Entities;
using Solution.DataBase;

namespace Solution.Services;

public class BookService(AppDbContext dbContext) : IBookService
{
    public async Task<ErrorOr<BookModel>> CreateAsync(BookModel book)
    {
        var books = await dbContext.Books.ToListAsync();

        if (books.Any(x => x.Writers == book.Writers.Value
       && x.Publisher == book.Publisher.Value
       && x.Title == book.Title.Value
       && x.ReleaseYear == book.ReleaseYear.Value))
        {
            return Error.Conflict(description: $"Book with the same data already exists.");
        }
        else if(books.Any(x => x.Id == book.Id.Value))
        {
            return Error.Conflict(description: $"Same ISBN code is already exists.");
        }

        BookEntity bookModel = book.ToEntity();

       await dbContext.Books.AddAsync(bookModel);
        await dbContext.SaveChangesAsync();

        return new BookModel(bookModel);
    }
}
