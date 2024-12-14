namespace Solution.Services;

public class MovieService(AppDbContext dbContext) : IMovieService
{
    public async Task CreateAsync(MovieModel movie)
    { 
        MovieEntity entity = movie.ToEntity();
        entity.PublicId = Guid.NewGuid().ToString();

        await dbContext.Movies.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }
}
