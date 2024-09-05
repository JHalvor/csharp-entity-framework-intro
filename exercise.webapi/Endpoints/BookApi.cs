using exercise.webapi.DTO;
using exercise.webapi.Models;
using exercise.webapi.Repository;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace exercise.webapi.Endpoints
{
    public static class BookApi
    {
        public static void ConfigureBooksApi(this WebApplication app)
        {
            var books = app.MapGroup("books");

            books.MapGet("/books", GetBooks);
            books.MapPut("/{id}", ChangeBookAuthor);
            books.MapDelete("/{id}", DeleteBook);
            books.MapPost("/", AddABook);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetBooks(IBookRepository Repository)
        {
            var books = await Repository.GetAllBooks();
            return TypedResults.Ok(books);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> ChangeBookAuthor(IBookRepository repository, int id, AuthorPutModel model)
        {
            try
            {
                var target = await repository.GetBookById(id);
                target.Author = new Author() { Id = target.AuthorId, FirstName=model.FirstName, LastName=model.LastName, Email=model.Email };
                await repository.UpdateById(id, target);
                return TypedResults.Ok(target);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> DeleteBook(IBookRepository repository, int id)
        {
            try
            {
                var target = await repository.DeleteById(id);
                return TypedResults.Ok(target);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> AddABook(IBookRepository repository, BookPostModel model)
        {
            try
            {
                var authorTarget = repository.GetAuthorById(model.AuthorId);
                if (authorTarget is null)
                {
                    return TypedResults.NotFound("Author Not Found");
                }
                var result = await repository.AddBook(new Book() { Title=model.Title, AuthorId=model.AuthorId });
                return TypedResults.Ok(result);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest("Invalid book object");
            }
        }
    }
}
