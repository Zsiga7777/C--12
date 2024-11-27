
namespace ChineseKreta.Database.Entities;

[Table("City")]
public class CityEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public uint PostalCode { get; set; }

    [Required]
    [StringLength(60)]
    public string Name { get; set; }
    public uint CountryId { get; set; }
    public virtual CountryEntity Country { get; set; }
    public ICollection<StreetEntity> Streets { get; set; }

   
}
