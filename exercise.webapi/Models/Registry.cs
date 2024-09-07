using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.webapi.Models
{
    [Table("registry")]
    public class Registry
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("authorFk")]
        public int AuthorId { get; set; }

        public Author Author { get; set; }

        [ForeignKey("bookFk")]
        public int BookId { get; set; }

        public Book Book { get; set; }
    }
}
