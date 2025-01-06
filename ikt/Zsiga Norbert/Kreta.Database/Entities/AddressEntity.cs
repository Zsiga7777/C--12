namespace Kreta.Database.Entities;

[Table("Address")]
public class AddressEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    [Required]
    [StringLength(60)]
    public string Address { get; set; }

    public uint StreetId { get; set; }

    public virtual StreetEntity Street { get; set; }

    public IReadOnlyCollection<StudentEntity> Students{ get; set; }
}
