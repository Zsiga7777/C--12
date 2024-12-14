namespace Solution.Core.Models;

[ObservableObject]
public partial class MovieModel
{
    [ObservableProperty]
    private ValidatableObject<string> id;

    [ObservableProperty]
    private ValidatableObject<string> title;

    [ObservableProperty]
    private ValidatableObject<uint?> length;

    [ObservableProperty]
    private ValidatableObject<DateTime> release;

    public MovieModel()
    {
    }

    public MovieModel(MovieEntity entity)
    {
        Id = entity.PublicId;
        Title = entity.Title;
        Length = entity.Length;
        Release = entity.Release;
    }

    public MovieEntity ToEntity()
    {
        return new MovieEntity
        {
            PublicId = Id,
            Title = Title,
            Length = Length.HasValue ? Length.Value : 0,
            Release = Release,
        };

    }

    public void ToEntity(MovieEntity entity)
    { 
        entity.PublicId = Id;
        entity.Title = Title;
        entity.Length = Length.HasValue ? Length.Value : 0;
        entity.Release = Release;

    }
}
