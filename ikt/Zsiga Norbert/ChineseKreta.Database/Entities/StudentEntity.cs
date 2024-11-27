namespace ChineseKreta.Database.Entities;

[Table("Student")]
public class StudentEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint EducationalID { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    public DateTime BirthDay { get; set; }

    [Required]
    [StringLength (50)]
    public string MothersName { get; set; }
    public uint AddressId { get; set; }
    public virtual AddressEntity Address { get; set; }

    public ICollection<MarkEntity> Marks { get; set; }
}
