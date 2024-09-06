﻿using exercise.webapi.DTO;
using exercise.webapi.Models;
using exercise.webapi.Repository;
using Microsoft.AspNetCore.Mvc;
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
            books.MapPut("/{id}", ChangeBookAuthor);
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public static async Task<IResult> ChangeBookAuthor(IBookRepository bookRepository, IAuthorRepository authorRepository, int bookId, int authorId)
        {
            try
            {
                var authorTarget = await authorRepository.GetById(authorId);
                if (authorTarget is null)
                {
                    return TypedResults.NotFound("Author Not Found");
                }

                var bookTarget = await bookRepository.GetById(bookId);
                if (bookTarget is null)
                {
                    return TypedResults.NotFound("Book Not Found");
                }

                var updatedTarget = await bookRepository.UpdateById(bookId, authorId);

                // Custom DTO
                BookEndpointResponseBook responseBook = MakeResponseBookDTO(updatedTarget);
                return TypedResults.Ok(responseBook);
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

                //custom DTO
                BookEndpointResponseBook responseBook = MakeResponseBookDTO(target);
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
        public static async Task<IResult> AddABook(IBookRepository bookRepository, IAuthorRepository authorRepository, BookPostModel model)
        {
            try
            {
                var authorTarget = await authorRepository.GetById(model.AuthorId);
                if (authorTarget is null)
                {
                    return TypedResults.NotFound("Author Not Found");
                }
                var result = await bookRepository.Add(new Book() { Title=model.Title, AuthorId=model.AuthorId });
                var target = await bookRepository.GetById(result.Id);
                BookEndpointResponseBook responseBook = MakeResponseBookDTO(target);
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

            BookEndpointResponseAuthor author = new BookEndpointResponseAuthor();
            author.Id = book.Author.Id;
            author.FirstName = book.Author.FirstName;
            author.LastName = book.Author.LastName;
            author.Email = book.Author.Email;

            responseBook.Author = author;

            return responseBook;
        }
    }
}
