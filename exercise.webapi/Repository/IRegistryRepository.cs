using exercise.webapi.Models;

namespace exercise.webapi.Repository
{
    public interface IRegistryRepository
    {
        public Task<IEnumerable<Registry>> GetRegistriesBookId(int id);
        public Task<IEnumerable<Registry>> GetRegistriesAuthorId(int id);
        public Task<Registry> DeleteById(int bookId, int authorId);
        public Task<Registry> Add(Registry entity);
    }
}
