﻿using exercise.webapi.DTO;
using exercise.webapi.Models;
using exercise.webapi.Repository;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Net;
using static System.Reflection.Metadata.BlobBuilder;
using System.Linq;

namespace exercise.webapi.Endpoints
{
    public static class BookEndpoint
    {
        public static void ConfigureBookEndpoints(this WebApplication app)
        {
            var books = app.MapGroup("books");

            books.MapGet("/", GetBooks);
            books.MapGet("/{id}", GetABook);
            books.MapPut("/assignAuthor/", AssignAuthor);
            books.MapPut("/removeAuthor/", RemoveAuthor);
            books.MapDelete("/{id}", DeleteBook);
            books.MapPost("/", AddABook);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        public static async Task<IResult> GetBooks(IBookRepository repository)
        {
            //custom DTO
            GetBooksResponse response = new GetBooksResponse();

            var results = await repository.GetAllBooks();

            foreach (Book b in results)
            {
                BookEndpointResponseBook responseBook = MakeResponseBookDTO(b);

                response.Books.Add(responseBook);
            }

            return TypedResults.Ok(response);
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> GetABook(IBookRepository repository, int id)
        {
            try
            {
                var target = await repository.GetById(id);
                if (target is null)
                {
                    return TypedResults.NotFound("Book Not Found");
                }

                BookEndpointResponseBook responseBook = MakeResponseBookDTO(target);

                return TypedResults.Ok(responseBook);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest("Invalid book object");
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> AssignAuthor(IBookRepository bookRepository, IAuthorRepository authorRepository, IRegistryRepository registryRepository, int bookId, int authorId)
        {
            try
            {
                var bookTarget = await bookRepository.GetById(bookId);
                if (bookTarget is null)
                {
                    return TypedResults.NotFound("Book Not Found");
                }

                var authorTarget = await authorRepository.GetById(authorId);
                if (authorTarget is null)
                {
                    return TypedResults.NotFound("Author Not Found");
                }

                var registryResult = await registryRepository.Add(new Registry() { BookId = bookId, AuthorId = authorId });
                var updatedBookResult = await bookRepository.GetById(bookId);

                // Custom DTO
                BookEndpointResponseBook responseBook = MakeResponseBookDTO(updatedBookResult);
                return TypedResults.Ok(responseBook);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public static async Task<IResult> RemoveAuthor(IBookRepository bookRepository, IAuthorRepository authorRepository, IRegistryRepository registryRepository, int bookId, int authorId)
        {
            try
            {
                var bookTarget = await bookRepository.GetById(bookId);
                if (bookTarget is null)
                {
                    return TypedResults.NotFound("Book Not Found");
                }

                var authorTarget = await authorRepository.GetById(authorId);
                if (authorTarget is null)
                {
                    return TypedResults.NotFound("Author Not Found");
                }

                var deletedRegistry = await registryRepository.DeleteById(bookId, authorId);
                if (deletedRegistry is null)
                {
                    return TypedResults.NotFound("Author not assigned to book");
                }

                var updatedBookResult = await bookRepository.GetById(bookId);

                // Custom DTO
                BookEndpointResponseBook responseBook = MakeResponseBookDTO(updatedBookResult);
                return TypedResults.Ok(responseBook);
            }
            catch (Exception ex)
            {
                return TypedResults.Problem(ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> DeleteBook(IBookRepository bookRepository, IRegistryRepository registryRepository, int id)
        {
            try
            {
                var target = await bookRepository.DeleteById(id);
                if (target is null)
                {
                    return TypedResults.NotFound("Book Not Found");
                }

                //custom DTO
                BookEndpointResponseBook responseBook = MakeResponseBookDTO(target);

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

                var newBook = await bookRepository.Add(new Book() { Title = model.Title });

                // Creating registry relations between the created book and its authors
                foreach (int authorId in model.AuthorIds)
                {
                    await registryRepository.Add(new Registry() { BookId = newBook.Id, AuthorId = authorId });
                }

                BookEndpointResponseBook responseBook = MakeResponseBookDTO(newBook);
                return TypedResults.Created($"https://localhost:7054/books/{responseBook.Id}", responseBook);
            }
            catch (Exception ex)
            {
                return TypedResults.BadRequest("Invalid book object");
            }
        }

        public static BookEndpointResponseBook MakeResponseBookDTO(Book book)
        {
            BookEndpointResponseBook responseBook = new BookEndpointResponseBook();
            responseBook.Title = book.Title;
            responseBook.Id = book.Id;

            foreach (Author a in book.Authors)
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
    }
}
