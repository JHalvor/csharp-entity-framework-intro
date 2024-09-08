using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.webapi.Models
{
    [Table("authorBook")]
    public class AuthorBook
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
