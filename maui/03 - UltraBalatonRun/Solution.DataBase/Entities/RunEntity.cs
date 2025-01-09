using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Solution.Database.Entities;

[Table("Run")]
public class RunEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    [Required]
    [StringLength(64)]
    public string PublicId { get; set; }

    [Required]
    public DateTime Date {  get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public double Distance { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public double AverageSpeed { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public double BurntCalories { get; set; }

    [Required]
    public uint RunningTime { get; set; }
}
