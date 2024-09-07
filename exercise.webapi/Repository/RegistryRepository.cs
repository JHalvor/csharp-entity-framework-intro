using exercise.webapi.Data;
using exercise.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.webapi.Repository
{
    public class RegistryRepository : IRegistryRepository
    {
        DataContext _db;

        public RegistryRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<Registry> Add(Registry entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Registry> DeleteById(int bookId, int authorId)
        {
            var target = await _db.Registries.FirstOrDefaultAsync(r => r.BookId == bookId && r.AuthorId == authorId);
            _db.Registries.Remove(target);

            await _db.SaveChangesAsync();
            return target;
        }

        public async Task<Registry> GetByAuthorId(int id)
        {
            return await _db.Registries.FirstOrDefaultAsync(r => r.AuthorId == id);
        }

        public async Task<IEnumerable<Registry>> GetRegistriesAuthorId(int id)
        {
            return await _db.Registries.Where(r => r.AuthorId == id).ToListAsync();
        }

        public async Task<IEnumerable<Registry>> GetRegistriesBookId(int id)
        {
            return await _db.Registries.Where(r => r.BookId == id).ToListAsync();
        }
    }
}
