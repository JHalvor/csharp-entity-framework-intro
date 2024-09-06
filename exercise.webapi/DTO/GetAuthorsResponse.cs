namespace exercise.webapi.DTO
{
    public class GetAuthorsResponse
    {
        public List<AuthorEndpointResponseAuthor> Authors { get; set; } = new List<AuthorEndpointResponseAuthor>();
    }
}
