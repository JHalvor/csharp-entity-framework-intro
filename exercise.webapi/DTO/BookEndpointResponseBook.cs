namespace exercise.webapi.DTO
{
    public class BookEndpointResponseBook
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public BookEndpointResponseAuthor Author { get; set; }
    }
}
