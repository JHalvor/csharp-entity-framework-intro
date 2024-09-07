using exercise.webapi.Models;

namespace exercise.webapi.Repository
{
    public interface IAuthorRepository
    {
        public Task<IEnumerable<Author>> GetAllAuthors();

        public Task<Author> GetById(int id);

        public Task<Author> Add(Author entity);

        public Task<Author> UpdateById(int id, Author entity);

        public Task<Author> DeleteById(int id);
    }
}
