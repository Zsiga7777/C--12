namespace Solution.DataBase;

public class AppDbContext() : DbContext
{
	public DbSet<ManufacturerEntity> Manufacturers { get; set; }

	public DbSet<MotorcycleEntity> Motorcycles { get; set; }

	private static string connectionString = string.Empty;

	static AppDbContext()
	{
		connectionString = GetConnectionString();
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		ArgumentNullException.ThrowIfNull(connectionString);

		base.OnConfiguring(optionsBuilder);

		optionsBuilder.UseSqlServer(connectionString);
	}

	private static string GetConnectionString()
	{
#if DEBUG
		var file = "appSettings.Development.json";
#else
        var file = "connectionString.Production.json";
#endif
		var stream = new MemoryStream(File.ReadAllBytes($"{file}"));

		var config = new ConfigurationBuilder()
					.AddJsonStream(stream)
					.Build();

		var cs = config.GetValue<string>("SqlConnectionString");
		return cs;
	}

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<ManufacturerEntity>()
            .HasData(
                new ManufacturerEntity { Id = 1, Name = "Honda" },
                new ManufacturerEntity { Id = 2, Name = "Yamaha" },
                new ManufacturerEntity { Id = 3, Name = "Suzuki" },
                new ManufacturerEntity { Id = 4, Name = "Triumph" },
                 new ManufacturerEntity { Id = 5, Name = "Harley-Davidson" },
                new ManufacturerEntity { Id = 6, Name = "Kawasaki" }
            );
    }

}
