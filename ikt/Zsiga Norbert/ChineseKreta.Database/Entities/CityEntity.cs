
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

    public ICollection<AddressEntity> Addresses { get; set; }
}
