using exercise.webapi.DTO;
using exercise.webapi.Models;
using exercise.webapi.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using System.Net;

namespace exercise.webapi.Endpoints
{
    public static class AuthorEndpoint
    {
        public static void ConfigureAuthorEndpoints(this WebApplication app)
        {
            var authors = app.MapGroup("authors");

            authors.MapGet("/authors", GetAuthors);
            //authors.MapGet("/{id}", GetAAuthor);
            //authors.MapPut("/{id}", UpdateAuthor);
            //authors.MapDelete("/{id}", DeleteAuthor);
            //authors.MapPost("/", AddAAuthor);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetAuthors(IAuthorRepository repository)
        {
            //custom DTO
            GetAuthorsResponse response = new GetAuthorsResponse();

            var results = await repository.GetAllAuthors();

            foreach (Author a in results)
            {
                AuthorEndpointResponseAuthor responseAuthor = MakeResponseAuthorDTO(a);

                response.Authors.Add(responseAuthor);
            }

            return TypedResults.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetAAuthor(IAuthorRepository repository, int id)
        {
            try
            {
                var target = await repository.GetById(id);
                if (target is null)
                {
                    return TypedResults.NotFound("Author Not Found");
                }

                AuthorEndpointResponseAuthor responseAuthor = MakeResponseAuthorDTO(target);

                return TypedResults.Ok(responseAuthor);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest("Invalid Author object");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> UpdateAuthor(IAuthorRepository repository, IRegistryRepository registryRepository, int id, AuthorPostModel model)
        {
            try
            {
                var target = await repository.GetById(id);
                if (target is null)
                {
                    return TypedResults.NotFound("Author Not Found");
                }

                var registries = await registryRepository.GetRegistriesByAuthorId(id);
                foreach (Registry registry in registries)
                {
                    if (!model.BookIds.Contains(registry.BookId))
                    {
                        await registryRepository.DeleteById(registry.BookId, id);
                    }
                }

                foreach (int bookID in model.BookIds)
                {
                    if (!registries.Select(r => r.BookId).Contains(bookID))
                    {
                        await registryRepository.Add(new Registry () { BookId = bookID });
                    }
                }

                var updatedTarget = await repository.UpdateById(id, new Author() { FirstName = model.FirstName, LastName = model.LastName, Email = model.Email });

                // Custom DTO
                AuthorEndpointResponseAuthor responseAuthor = MakeResponseAuthorDTO(updatedTarget);
                return TypedResults.Ok(responseAuthor);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public static async Task<IResult> DeleteAuthor(IAuthorRepository repository, int id)
        //{
        //    try
        //    {
        //        var target = await repository.DeleteById(id);

        //        //custom DTO
        //        AuthorEndpointResponseAuthor responseAuthor = MakeResponseAuthorDTO(target);
        //        return TypedResults.Ok(responseAuthor);
        //    }
        //    catch (Exception ex)
        //    {
        //        return TypedResults.Problem(ex.Message);
        //    }
        //}

        //[ProducesResponseType(StatusCodes.Status201Created)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //public static async Task<IResult> AddAAuthor(IAuthorRepository repository, AuthorPostModel model)
        //{
        //    try
        //    {
        //        var result = await repository.Add(new Author() { FirstName = model.FirstName, LastName = model.LastName, Email = model.Email });
        //        var target = await repository.GetById(result.Id);
        //        AuthorEndpointResponseAuthor responseAuthor = MakeResponseAuthorDTO(target);
        //        return TypedResults.Created($"https://localhost:7054/authors/{responseAuthor.Id}", responseAuthor);
        //    }
        //    catch (Exception ex)
        //    {
        //        return TypedResults.BadRequest("Invalid author object");
        //    }
        //}

        public static AuthorEndpointResponseAuthor MakeResponseAuthorDTO(Author author)
        {
            AuthorEndpointResponseAuthor responseAuthor = new AuthorEndpointResponseAuthor();
            responseAuthor.Id = author.Id;
            responseAuthor.FirstName = author.FirstName;
            responseAuthor.LastName = author.LastName;
            responseAuthor.Email = author.Email;

            foreach (Book b in author.Books)
            {
                AuthorEndpointResponseBook responseBook = new AuthorEndpointResponseBook();
                responseBook.Id = b.Id;
                responseBook.Title = b.Title;
                responseAuthor.Books.Add(responseBook);
            }

            return responseAuthor;
        }
    }
}
