namespace Solution.Database.Entities
{
    [Table("Book")]
    public class BookEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public ulong Id { get; set; }

        [Required]
        public string Writers { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public uint ReleaseYear { get; set; }

        [Required]
        public string Publisher { get; set; }

    }
}
