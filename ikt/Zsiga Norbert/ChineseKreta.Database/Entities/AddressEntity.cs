namespace ChineseKreta.Database.Entities;

[Table("Address")]
public class AddressEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    [Required]
    [StringLength(60)]
    public string Address { get; set; }

    public uint CityId { get; set; }

    public virtual CityEntity City { get; set; }

    public uint StudentId { get; set; }
    public virtual StudentEntity Student { get; set; }
}
