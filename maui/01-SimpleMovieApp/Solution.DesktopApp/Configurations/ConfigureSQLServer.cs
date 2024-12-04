
namespace Solution.DesktopApp.Configurations;

public static class ConfigureSQLServer
{
    public static MauiAppBuilder UseMsSqlServer(this MauiAppBuilder builder)
    {
        string connectionString = builder.Configuration.GetRequiredSection("SqlConnectionString").Get<string>();

        ArgumentNullException.ThrowIfNull(connectionString);

        builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        
        return builder;
    }
}
