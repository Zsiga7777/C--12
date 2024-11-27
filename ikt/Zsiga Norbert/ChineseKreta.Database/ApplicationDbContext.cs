using ChineseKreta.Database.Entities;

namespace ChineseKreta.Database;

public class ApplicationDbContext : DbContext
{
    public DbSet<AddressEntity> Addresses { get; set; }
    public DbSet<CityEntity> Cities { get; set; }
    public DbSet<CountryEntity> Countries { get; set; }
    public DbSet<MarkEntity> Marks { get; set; }
    public DbSet<StreetEntity> Streets { get; set; }
    public DbSet<StudentEntity> Students { get; set; }
    public DbSet<SubjectEntity> Subjects { get; set; }
    public ApplicationDbContext() : base()
    { 
    
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ChineseKretaDB;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True;");
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      
    }
}

