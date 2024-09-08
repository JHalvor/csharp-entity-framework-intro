using exercise.webapi.Data;
using exercise.webapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace exercise.webapi.Repository
{
    public class AuthorRepository : IAuthorRepository
    {
        private DataContext _db;

        public AuthorRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Author>> GetAllAuthors()
        {
            return await _db.Authors.Include(a => a.Books).ToListAsync();
        }

        public async Task<Author> GetById(int id)
        {
            return await _db.Authors.Include(a => a.Books).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Author> Add(Author entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Author> UpdateById(int id, Author entity)
        {
            var target = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);
            target.FirstName = entity.FirstName;
            target.LastName = entity.LastName;
            target.Email = entity.Email;
            _db.Attach(target).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return target;
        }

        public async Task<Author> DeleteById(int id)
        {
            var target = await _db.Authors.FirstOrDefaultAsync(a => a.Id == id);
            _db.Authors.Remove(target);
            await _db.SaveChangesAsync();
            return target;
        }
    }
}
