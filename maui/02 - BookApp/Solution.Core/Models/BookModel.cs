using MauiValidationLibrary;
using MauiValidationLibrary.ValidationRules;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Solution.Database.Entities;
using Solution.ValidationLibrary.ValidationRules;
using System.ComponentModel.DataAnnotations;

namespace Solution.Core.Models;

public partial class BookModel
{
    public ValidatableObject<ulong> Id { get; set; }

    public ValidatableObject<string> Writers { get; protected set; }
    public ValidatableObject<string> Title { get; protected set; }
    public ValidatableObject<uint?> ReleaseYear { get; protected set; }
    public ValidatableObject<string> Publisher { get; protected set; }
   
    public BookModel() 
    {
        this.Id = new ValidatableObject<ulong>();
        this.Writers = new ValidatableObject<string>();
        this.Title = new ValidatableObject<string>();
        this.ReleaseYear = new ValidatableObject<uint?>();
        this.Publisher = new ValidatableObject<string>();

        AddValidators();
    }

    public BookModel(BookEntity entity): this()
    { 
        Id.Value = entity.Id;
        Writers.Value = entity.Writers;
        Title.Value = entity.Title;
        ReleaseYear.Value = entity.ReleaseYear;
        Publisher.Value = entity.Publisher;
    }

    public BookEntity ToEntity()
    {
        return new BookEntity
        {
            Id = Id.Value,
            Writers = Writers.Value,
            Title = Title.Value,
            ReleaseYear = ReleaseYear.Value ?? 0,
            Publisher = Publisher.Value
        };
    }

    public void ToEntity(BookEntity entity)
    { 
        entity.Id = Id.Value;
        entity.Writers = Writers.Value;
        entity.Title = Title.Value;
        entity.ReleaseYear = ReleaseYear.Value ?? 0;
        entity.Publisher = Publisher.Value;
    }

    private void AddValidators()
    {
        this.Writers.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Writer(s) is required field." });
        this.Title.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Title is required field." });
        this.Publisher.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Publisher is required field." });
        this.ReleaseYear.Validations.Add(new NullableIntegerRule<uint?> { ValidationMessage = "ReleaseYear is required field." });
        this.ReleaseYear.Validations.Add(new MinValueRule<uint?>(1) { ValidationMessage = "Release year can't be less than 1" });
        this.ReleaseYear.Validations.Add(new MaxValueRule<uint?>(DateTime.Now.Year) { ValidationMessage = $"Release year can't be more than {DateTime.Now.Year}" });
        this.Id.Validations.Add(new IsValidISBNCodeRule<ulong> { ValidationMessage = "Id must be a 11 or 13 character long number."});
    }
}
