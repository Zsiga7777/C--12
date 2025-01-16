using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Database.Entities;

[Table("Motorcycle")]
public class MotorcycleEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    [Required]
    [StringLength(128)]
    public string PublicId { get; set; }

    [Required]
    [StringLength(128)]
    public string Model { get; set; }

    [Required]
    public uint Cubic {  get; set; }

    [Required]
    public uint ReleaseYear { get; set; }

    [Required]
    public uint Cylinders { get; set; }

    public uint ManufacturerId { get; set; }
    public virtual ManufacturerEntity Manufacturer { get; set; }
}
