using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.webapi.Models
{
    [Table("author")]
    public class Author
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("firstname")]
        public string FirstName { get; set; }

        [Column("lastname")]
        public string LastName { get; set; }

        [Column("email")]
        public string Email { get; set; }

        public List<Book> Books { get; set; } = new List<Book>();
    }
}
