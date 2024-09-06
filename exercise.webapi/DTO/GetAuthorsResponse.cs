namespace exercise.webapi.DTO
{
    public class GetAuthorsResponse
    {
        public List<ResponseAuthor> Authors { get; set; } = new List<ResponseAuthor>();
    }
}
