using exercise.webapi.DTO;
using exercise.webapi.Models;
using exercise.webapi.Repository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;

namespace exercise.webapi.Endpoints
{
    public static class BookEndpoint
    {
        public static void ConfigureBookEndpoints(this WebApplication app)
        {
            var books = app.MapGroup("books");

            books.MapGet("/books", GetBooks);
            books.MapGet("/{id}", GetABook);
            //books.MapPut("/{id}", UpdateBook);
            books.MapDelete("/{id}", DeleteBook);
            books.MapPost("/", AddABook);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetBooks(IBookRepository bookRepository, IAuthorRepository authorRepository, IRegistryRepository registryRepository)
        {
            //custom DTO
            GetBooksResponse response = new GetBooksResponse();

            var results = await bookRepository.GetAllBooks();

            foreach (Book b in results)
            {
                List<Author> authors = await GetAuthorsByBookId(registryRepository, authorRepository, b.Id);

                BookEndpointResponseBook responseBook = MakeResponseBookDTO(b, authors);

                response.Books.Add(responseBook);
            }

            return TypedResults.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetABook(IBookRepository bookRepository, IRegistryRepository registryRepository, IAuthorRepository authorRepository, int id)
        {
            try
            {
                var target = await bookRepository.GetById(id);
                if (target is null)
                {
                    return TypedResults.NotFound("Book Not Found");
                }

                List<Author> authors = await GetAuthorsByBookId(registryRepository, authorRepository, id);

                BookEndpointResponseBook responseBook = MakeResponseBookDTO(target, authors);

                return TypedResults.Ok(responseBook);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest("Invalid book object");
            }
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public static async Task<IResult> UpdateBook(IBookRepository bookRepository, IAuthorRepository authorRepository, int bookId, int authorId)
        //{
        //    try
        //    {
        //        var authorTarget = await authorRepository.GetById(authorId);
        //        if (authorTarget is null)
        //        {
        //            return TypedResults.NotFound("Author Not Found");
        //        }

        //        var bookTarget = await bookRepository.GetById(bookId);
        //        if (bookTarget is null)
        //        {
        //            return TypedResults.NotFound("Book Not Found");
        //        }

        //        var updatedTarget = await bookRepository.UpdateById(bookId, authorId);

        //        // Custom DTO
        //        BookEndpointResponseBook responseBook = MakeResponseBookDTO(updatedTarget);
        //        return TypedResults.Ok(responseBook);
        //    }
        //    catch (Exception ex)
        //    {
        //        return TypedResults.Problem(ex.Message);
        //    }
        //}

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> DeleteBook(IBookRepository repository, IAuthorRepository authorRepository, IRegistryRepository registryRepository, int id)
        {
            try
            {
                var target = await repository.DeleteById(id);
                if (target is null)
                {
                    return TypedResults.NotFound("Book Not Found");
                }
                List<Author> authors = await GetAuthorsByBookId(registryRepository, authorRepository, id);
                

                //custom DTO
                BookEndpointResponseBook responseBook = MakeResponseBookDTO(target, authors);

                // Deleting relations between the deleted book and authors
                var registries = await registryRepository.GetRegistriesByBookId(id);
                registries.ToList().ForEach(async r => await registryRepository.DeleteById(id, r.AuthorId));

                return TypedResults.Ok(responseBook);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> AddABook(IBookRepository bookRepository, IAuthorRepository authorRepository, IRegistryRepository registryRepository, BookPostModel model)
        {
            try
            {
                foreach (int authorId in model.AuthorIds)
                {
                    var authorTarget = await authorRepository.GetById(authorId);
                    if (authorTarget is null)
                    {
                        return TypedResults.NotFound("Author Not Found");
                    }
                }
                
                var newBook = await bookRepository.Add(new Book() { Title=model.Title });

                // Creating registry relations between the created book and its authors
                var registries = await registryRepository.GetRegistriesByBookId(newBook.Id);
                registries.ToList().ForEach(async r => await registryRepository.Add(new Registry() { BookId=newBook.Id, AuthorId=r.AuthorId }));

                List<Author> authors = await GetAuthorsByBookId(registryRepository, authorRepository, newBook.Id);

                BookEndpointResponseBook responseBook = MakeResponseBookDTO(newBook, authors);
                return TypedResults.Created($"https://localhost:7054/books/{responseBook.Id}", responseBook);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest("Invalid book object");
            }
        }

        public static BookEndpointResponseBook MakeResponseBookDTO(Book book, List<Author> authors)
        {
            BookEndpointResponseBook responseBook = new BookEndpointResponseBook();
            responseBook.Title = book.Title;
            responseBook.Id = book.Id;

            foreach (Author a in authors)
            {
                BookEndpointResponseAuthor author = new BookEndpointResponseAuthor();
                author.Id = a.Id;
                author.FirstName = a.FirstName;
                author.LastName = a.LastName;
                author.Email = a.Email;
                responseBook.Authors.Add(author);
            }

            return responseBook;
        }

        public static async Task<List<Author>> GetAuthorsByBookId(IRegistryRepository registryRepository, IAuthorRepository authorRepository, int bookId)
        {
            var registries = await registryRepository.GetRegistriesByBookId(bookId);

            List<Author> authors = new List<Author>();
            foreach (var registry in registries)
            {
                authors.Add(await authorRepository.GetById(registry.AuthorId));
            }
            return authors;
        }
    }
}
