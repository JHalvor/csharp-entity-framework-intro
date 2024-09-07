namespace exercise.webapi.DTO
{
    public class BookEndpointResponseBook
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public List<BookEndpointResponseAuthor> Authors { get; set; } = new List<BookEndpointResponseAuthor>();
    }
}
