using exercise.webapi.Data;
using exercise.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace exercise.webapi.Repository
{
    public class AuthorBookRepository : IAuthorBookRepository
    {
        DataContext _db;

        public AuthorBookRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<AuthorBook> Add(AuthorBook entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<AuthorBook> DeleteById(int bookId, int authorId)
        {
            var target = await _db.AuthorBooks.FirstOrDefaultAsync(r => r.BookId == bookId && r.AuthorId == authorId);
            if (target == null)
            {
                return null;
            }
            _db.AuthorBooks.Remove(target);

            await _db.SaveChangesAsync();
            return target;
        }

        public async Task<IEnumerable<AuthorBook>> GetAuthorBooksByBookId(int id)
        {
            return await _db.AuthorBooks.Where(ab => ab.BookId == id).ToListAsync();
        }

        public async Task<IEnumerable<AuthorBook>> GetAuthorBooksByAuthorId(int id)
        {
            return await _db.AuthorBooks.Where(r => r.AuthorId == id).ToListAsync();
        }
    }
}
