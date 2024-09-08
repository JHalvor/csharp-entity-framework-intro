using exercise.webapi.Models;

namespace exercise.webapi.Repository
{
    public interface IAuthorBookRepository
    {
        public Task<AuthorBook> Add(AuthorBook entity);
        public Task<AuthorBook> DeleteById(int bookId, int authorId);
        public Task<IEnumerable<AuthorBook>> GetAuthorBooksByBookId(int id);
        public Task<IEnumerable<AuthorBook>> GetAuthorBooksByAuthorId(int id);
    }
}
