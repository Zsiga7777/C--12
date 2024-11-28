
namespace Solution.DesktopApp.Configurations;

public static class ConfigureSQLServer
{
    public static MauiAppBuilder UseMsSqlServer(this MauiAppBuilder builder)
    {
        string connectionString = builder.Configuration.GetConnectionString("MvcMovieContext");

        ArgumentNullException.ThrowIfNull(connectionString);

        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        
        return builder;
    }
}
