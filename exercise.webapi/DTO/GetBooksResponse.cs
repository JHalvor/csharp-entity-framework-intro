namespace exercise.webapi.DTO
{
    public class GetBooksResponse
    {
        public List<ResponseBook> Books { get; set; } = new List<ResponseBook>();
    }
}
