using exercise.webapi.DTO;
using exercise.webapi.Models;
using exercise.webapi.Repository;
using Microsoft.AspNetCore.Mvc;

namespace exercise.webapi.Endpoints
{
    public static class AuthorEndpoint
    {
        public static void ConfigureAuthorEndpoints(this WebApplication app)
        {
            var authors = app.MapGroup("authors");

            authors.MapGet("/authors", GetAuthors);
            //books.MapGet("/{id}", GetAAuthor);
            //books.MapPut("/{id}", UpdateAuthor);
            //books.MapDelete("/{id}", DeleteAuthor);
            //books.MapPost("/", AddAAuthor);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAuthors(IAuthorRepository repository)
        {
            //custom DTO
            GetAuthorsResponse response = new GetAuthorsResponse();

            var results = await repository.GetAllAuthors();

            foreach (Author a in results)
            {
                AuthorEndpointResponseAuthor responseAuthor = new AuthorEndpointResponseAuthor();
                responseAuthor.Id = a.Id;
                responseAuthor.FirstName = a.FirstName;
                responseAuthor.LastName = a.LastName;
                responseAuthor.Email = a.Email;

                foreach (Book b in a.Books)
                {
                    AuthorEndpointResponseBook responseBook = new AuthorEndpointResponseBook();
                    responseBook.Id = b.Id;
                    responseBook.Title = b.Title;
                    responseAuthor.Books.Add(responseBook);
                }

                response.Authors.Add(responseAuthor);
            }

            return TypedResults.Ok(response);
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public static async Task<IResult> GetAAuthor(IBookRepository repository, int id)
        //{
        //    try
        //    {
        //        var target = await repository.GetById(id);
        //        if (target is null)
        //        {
        //            return TypedResults.NotFound("Book Not Found");
        //        }

        //        ResponseBook responseBook = MakeResponseBookDTO(target);

        //        return TypedResults.Ok(responseBook);
        //    }
        //    catch (Exception ex)
        //    {
        //        return TypedResults.BadRequest("Invalid book object");
        //    }
        //}

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public static async Task<IResult> DeleteAuthor(IBookRepository repository, int id, AuthorPutModel model)
        //{
        //    try
        //    {
        //        var target = await repository.GetById(id);
        //        target.Author = new Author() { Id = target.AuthorId, FirstName = model.FirstName, LastName = model.LastName, Email = model.Email };
        //        await repository.UpdateById(id, target);

        //        // Custom DTO
        //        ResponseBook responseBook = MakeResponseBookDTO(target);
        //        return TypedResults.Ok(responseBook);
        //    }
        //    catch (Exception ex)
        //    {
        //        return TypedResults.Problem(ex.Message);
        //    }
        //}

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public static async Task<IResult> AddAAuthor(IBookRepository repository, int id)
        //{
        //    try
        //    {
        //        var target = await repository.DeleteById(id);

        //        //custom DTO
        //        ResponseBook responseBook = MakeResponseBookDTO(target);

        //        return TypedResults.Ok(responseBook);
        //    }
        //    catch (Exception ex)
        //    {
        //        return TypedResults.Problem(ex.Message);
        //    }
        //}

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public static async Task<IResult> AddABook(IBookRepository bookRepository, IBookRepository authorRepository, BookPostModel model)
        //{
        //    try
        //    {
        //        var authorTarget = await authorRepository.GetById(model.AuthorId);
        //        if (authorTarget is null)
        //        {
        //            return TypedResults.NotFound("Author Not Found");
        //        }
        //        var result = await bookRepository.Add(new Book() { Title = model.Title, AuthorId = model.AuthorId });
        //        return TypedResults.Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return TypedResults.BadRequest("Invalid book object");
        //    }
        //}

        //public static ResponseBook MakeResponseBookDTO(Book book)
        //{
        //    ResponseBook responseBook = new ResponseBook();
        //    responseBook.Title = book.Title;
        //    responseBook.Id = book.Id;

        //    ResponseAuthor author = new ResponseAuthor();
        //    author.Id = book.Author.Id;
        //    author.FirstName = book.Author.FirstName;
        //    author.LastName = book.Author.LastName;
        //    author.Email = book.Author.Email;

        //    responseBook.Author = author;

        //    return responseBook;
        //}
    }
}
