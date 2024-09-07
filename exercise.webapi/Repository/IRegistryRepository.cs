using exercise.webapi.Models;

namespace exercise.webapi.Repository
{
    public interface IRegistryRepository
    {
        public Task<Registry> Add(Registry entity);
        public Task<Registry> DeleteById(int bookId, int authorId);
        public Task<IEnumerable<Registry>> GetRegistriesByBookId(int id);
        public Task<IEnumerable<Registry>> GetRegistriesByAuthorId(int id);
    }
}
