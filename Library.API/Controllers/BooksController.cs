﻿using AutoMapper;
using Library.API.Models;
using Library.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace Library.API.Controllers
{
    [Route("api/v{version:apiVersion}/authors/{authorId}/books")]
    [ApiController]
    [Produces("application/json", "application/xml")]
    //[ApiExplorerSettings(GroupName = "LibraryOpenApiSpecificicationBooks")]
    public class BooksController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public BooksController(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository ?? throw new ArgumentNullException(nameof(bookRepository));
            _authorRepository = authorRepository ?? throw new ArgumentNullException(nameof(authorRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<Book>>> GetBooks(
            Guid authorId)
        {
            if (!await _authorRepository.AuthorExistsAsync(authorId))
            {
                return NotFound();
            }

            var booksFromRepo = await _bookRepository.GetBooksAsync(authorId);
            return Ok(_mapper.Map<IEnumerable<Book>>(booksFromRepo));
        }
        /// <summary>
        /// obtener libro especifico por autor especifico
        /// </summary>
        /// <param name="authorId">id autor</param>
        /// <param name="bookId">id libro</param>
        /// <returns>datos del libro</returns>
        /// <response code="200">Retorna informacion de la solicitud encontrada</response>
        [HttpGet("{bookId}")]
        [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Book), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Book), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Book>> GetBook(Guid authorId, Guid bookId)
        {
            if (!await _authorRepository.AuthorExistsAsync(authorId))
            {
                return NotFound();
            }

            var bookFromRepo = await _bookRepository.GetBookAsync(authorId, bookId);
            if (bookFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Book>(bookFromRepo));
        }

        [HttpPost()]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Consumes("application/json", "application/xml")]
        public async Task<ActionResult<Book>> CreateBook(Guid authorId, BookForCreation bookForCreation)
        {
            if (!await _authorRepository.AuthorExistsAsync(authorId))
            {
                return NotFound();
            }

            var bookToAdd = _mapper.Map<Entities.Book>(bookForCreation);
            _bookRepository.AddBook(bookToAdd);
            await _bookRepository.SaveChangesAsync();

            return CreatedAtRoute(
                "GetBook",
                new { authorId, bookId = bookToAdd.Id },
                _mapper.Map<Book>(bookToAdd));
        }
    }
}