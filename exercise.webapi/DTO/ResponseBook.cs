namespace exercise.webapi.DTO
{
    public class ResponseBook
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public ResponseAuthor Author { get; set; }
    }
}
