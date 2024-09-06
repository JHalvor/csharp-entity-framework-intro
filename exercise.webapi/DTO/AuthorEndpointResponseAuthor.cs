namespace exercise.webapi.DTO
{
    public class AuthorEndpointResponseAuthor
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public List<AuthorEndpointResponseBook> Books { get; set; } = new List<AuthorEndpointResponseBook>();
    }
}
