namespace exercise.webapi.DTO
{
    public class GetBooksResponse
    {
        public List<DTOBook> Books { get; set; } = new List<DTOBook>();
    }
}
