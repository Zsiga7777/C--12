
namespace ChineseKreta.Database.Entities;
[Table("Country")]
[Index(nameof(Name), IsUnique = true)]
public class CountryEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public IReadOnlyCollection<CityEntity> Cities { get; set; }
}
