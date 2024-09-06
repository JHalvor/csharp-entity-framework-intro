namespace exercise.webapi.DTO
{
    public class GetBooksResponse
    {
        public List<BookEndpointResponseBook> Books { get; set; } = new List<BookEndpointResponseBook>();
    }
}
