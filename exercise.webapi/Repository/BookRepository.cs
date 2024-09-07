using exercise.webapi.Data;
using exercise.webapi.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace exercise.webapi.Repository
{
    public class BookRepository : IBookRepository
    {
        DataContext _db;
        
        public BookRepository(DataContext db)
        {
            _db = db;
        }

        public async Task<Book> Add(Book entity)
        {
            await _db.AddAsync(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<Book> DeleteById(int id)
        {
            var bookTarget = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
            _db.Books.Remove(bookTarget);

            await _db.SaveChangesAsync();
            return bookTarget;
        }

        public async Task<IEnumerable<Book>> GetAllBooks()
        {
            return await _db.Books.ToListAsync();
        }

        public async Task<Book> GetById(int id)
        {
            return await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> UpdateById(int id, Book entity)
        {
            var target = await _db.Books.FirstOrDefaultAsync(b => b.Id == id);
            target.Title = entity.Title;
            _db.Attach(target).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return target;
        }
    }
}
