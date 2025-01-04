﻿namespace ChineseKreta.Database.Entities;
[Table("Street")]
public class StreetEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public uint Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; }

    public uint CityId { get; set; }

    public virtual CityEntity City { get; set; }

    public IReadOnlyCollection<AddressEntity> Addresses { get; set; }
}
