namespace ChineseKreta.Database.Entities;

[Table("Mark")]
public class MarkEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint MarkId { get; set; }

    [Required]
    [Range(1,5)]
    public uint Mark { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public uint? StudentId { get; set; }

    public virtual StudentEntity? Student { get; set; }

    public uint SubjectId { get; set; }

    public virtual SubjectEntity Subject { get; set; }
}
