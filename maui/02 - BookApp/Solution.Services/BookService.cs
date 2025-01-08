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
       var isBookExists = await dbContext.Books.AnyAsync(x => x.Id == book.Id.Value
       ||( x.Writers == book.Writers.Value 
       && x.Publisher == book.Publisher.Value 
       && x.Title == book.Title.Value
       && x.ReleaseYear == book.ReleaseYear.Value));

        if (isBookExists)
        {
            return Error.Conflict(description: $"Book with the same data already exists.");
        }

        BookEntity bookModel = book.ToEntity();

       await dbContext.Books.AddAsync(bookModel);
        await dbContext.SaveChangesAsync();

        return new BookModel(bookModel);
    }
}
