namespace Solution.Core.Models;
public partial class MovieModel
{
    public ValidatableObject<string> Id { get; protected set; }

    public ValidatableObject<string> Title { get; protected set; }

    public ValidatableObject<uint?> Length { get; protected set; }

    public ValidatableObject<DateTime> Release { get; protected set; }

    public MovieModel()
    {
        this.Title = new ValidatableObject<string>();
        this.Length = new ValidatableObject<uint?>();
        this.Release = new ValidatableObject<DateTime>();

        AddValidators();
    }
    public MovieModel(MovieEntity entity) : this()
    {
        Id.Value = entity.PublicId;
        Title.Value = entity.Title;
        Length.Value = entity.Length;
        Release.Value = entity.Release;
    }

    public MovieEntity ToEntity()
    {
        return new MovieEntity
        {
            PublicId = Id.Value,
            Title = Title.Value,
            Length = Length.Value ??  0,
            Release = Release.Value,
        };

    }

    public void ToEntity(MovieEntity entity)
    { 
        entity.PublicId = Id.Value;
        entity.Title = Title.Value;
        entity.Length = Length.Value ?? 0;
        entity.Release = Release.Value;

    }

    private void AddValidators()
    {
        this.Title.Validations.Add(new IsNotNullOrEmptyRule<string>
        { 
        ValidationMessage = "Title is required field."
        });
        this.Length.Validations.Add(new NullableIntegerRule<uint?>
        {
            ValidationMessage = "Length is required field."
        });
    }
}
