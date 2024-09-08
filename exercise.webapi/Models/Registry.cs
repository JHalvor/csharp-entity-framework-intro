using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.webapi.Models
{
    [Table("registries")]
    public class Registry
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("AuthorId")]
        public int AuthorId { get; set; }

        [ForeignKey("BookId")]
        public int BookId { get; set; }
    }
}
