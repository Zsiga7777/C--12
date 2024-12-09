using Solution.Database.Entities;

namespace Solution.Core.Models;

[ObservableObject]
public partial class MovieModel
{
    [ObservableProperty]
    private string id;

    [ObservableProperty]
    private string title;

    [ObservableProperty]
    private uint length;

    [ObservableProperty]
    private DateTime release;

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
            Length = Length,
            Release = Release,
        };

    }

    public void ToEntity(MovieEntity entity)
    { 
        entity.PublicId = Id;
        entity.Title = Title;
        entity.Length = Length;
        entity.Release = Release;

    }
}
