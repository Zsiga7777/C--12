namespace ChineseKreta.Database.Entities;

[Table("Subject")]
[Index(nameof(Name), IsUnique = true)]
public class SubjectEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    [Required]
    [StringLength(30)]
    public string Name { get; set; }

    public ICollection<MarkEntity> Marks { get; set; }

}
