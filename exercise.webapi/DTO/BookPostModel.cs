namespace exercise.webapi.DTO
{
    public class BookPostModel
    {
        public string Title { get; set; }

        public List<int> AuthorIds { get; set; } = new List<int>();
    }
}
