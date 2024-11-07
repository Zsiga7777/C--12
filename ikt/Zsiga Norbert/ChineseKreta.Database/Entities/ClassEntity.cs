

namespace ChineseKreta.Database.Entities;

[Table("Class")]
public class ClassEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    [Required]
    [StringLength(30)]
    public string Name { get; set; }

    public ICollection<StudentEntity> Students { get; set; }
}
