using System.ComponentModel.DataAnnotations.Schema;

namespace exercise.webapi.DTO
{
    public class ResponseAuthor
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}
