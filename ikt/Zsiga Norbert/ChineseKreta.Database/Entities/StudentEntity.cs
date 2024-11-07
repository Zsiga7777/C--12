namespace ChineseKreta.Database.Entities;

[Table("Student")]
public class StudentEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public uint ClassId { get; set; }
    public virtual ClassEntity Class { get; set; }

    [ForeignKey("Address")]
    public uint AddressId { get; set; }
    public virtual AddressEntity Address { get; set; }

    public ICollection<MarkEntity> Marks { get; set; }
}
