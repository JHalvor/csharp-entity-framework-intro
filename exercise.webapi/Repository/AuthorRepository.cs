using exercise.webapi.Data;
using exercise.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.webapi.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private DataContext _db;

        public AuthorRepository(DataContext db)
        {
            _db = db;
        }

        public Task<Author> Add(Author entity)
        {
            throw new NotImplementedException();
        }

        public Task<Author> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Author>> GetAllAuthors()
        {
            return await _db.Authors.Include(a => a.Books).ToListAsync();
        }

        public Task<Author> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Author> UpdateById(int id, Author entity)
        {
            throw new NotImplementedException();
        }
    }
}
