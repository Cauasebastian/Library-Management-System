using AutoMapper;
using Library_Management_System.DTOs;
using Library_Management_System.Models;
using Library_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers;

[Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IAuthorService _authorService;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;
        private readonly ILogger<BooksController> _logger;

        public BooksController(
            IBookService bookService,
            IAuthorService authorService,
            ICategoryService categoryService,
            IMapper mapper,
            ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _authorService = authorService;
            _categoryService = categoryService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os livros.
        /// </summary>
        /// <returns>Lista de livros.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            var booksDto = _mapper.Map<IEnumerable<BookDto>>(books);
            return Ok(booksDto);
        }

        /// <summary>
        /// Obtém um livro pelo ID.
        /// </summary>
        /// <param name="id">ID do livro.</param>
        /// <returns>Livro correspondente ao ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                _logger.LogWarning("Livro com ID {Id} não encontrado.", id);
                return NotFound();
            }

            var bookDto = _mapper.Map<BookDto>(book);
            return Ok(bookDto);
        }

        /// <summary>
        /// Cria um novo livro.
        /// </summary>
        /// <param name="bookDto">Dados do livro a ser criado.</param>
        /// <returns>Livro criado.</returns>
        [HttpPost]
        public async Task<ActionResult<BookDto>> CreateBook([FromBody] BookDto bookDto)
        {
            // Verificar se o autor existe
            var author = await _authorService.GetAuthorByIdAsync(bookDto.AuthorId);
            if (author == null)
            {
                return BadRequest($"Autor com ID {bookDto.AuthorId} não encontrado.");
            }

            // Verificar se as categorias existem
            foreach (var categoryId in bookDto.CategoryIds)
            {
                var category = await _categoryService.GetCategoryByIdAsync(categoryId);
                if (category == null)
                {
                    return BadRequest($"Categoria com ID {categoryId} não encontrada.");
                }
            }

            var book = _mapper.Map<Book>(bookDto);
            // Configurar BookCategories
            book.BookCategories = new List<BookCategory>();
            foreach (var categoryId in bookDto.CategoryIds)
            {
                book.BookCategories.Add(new BookCategory
                {
                    CategoryId = categoryId
                });
            }

            await _bookService.CreateBookAsync(book);
            var createdBookDto = _mapper.Map<BookDto>(book);

            return CreatedAtAction(nameof(GetBook), new { id = book.Id }, createdBookDto);
        }

        /// <summary>
        /// Atualiza um livro existente.
        /// </summary>
        /// <param name="id">ID do livro a ser atualizado.</param>
        /// <param name="bookDto">Dados atualizados do livro.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto bookDto)
        {
            if (id != bookDto.Id)
            {
                return BadRequest("ID do livro na URL não corresponde ao ID no corpo.");
            }

            var existingBook = await _bookService.GetBookByIdAsync(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            // Verificar se o autor existe
            var author = await _authorService.GetAuthorByIdAsync(bookDto.AuthorId);
            if (author == null)
            {
                return BadRequest($"Autor com ID {bookDto.AuthorId} não encontrado.");
            }

            // Verificar se as categorias existem
            foreach (var categoryId in bookDto.CategoryIds)
            {
                var category = await _categoryService.GetCategoryByIdAsync(categoryId);
                if (category == null)
                {
                    return BadRequest($"Categoria com ID {categoryId} não encontrada.");
                }
            }

            _mapper.Map(bookDto, existingBook);

            // Atualizar BookCategories
            existingBook.BookCategories.Clear();
            foreach (var categoryId in bookDto.CategoryIds)
            {
                existingBook.BookCategories.Add(new BookCategory
                {
                    BookId = id,
                    CategoryId = categoryId
                });
            }

            await _bookService.UpdateBookAsync(existingBook);

            return NoContent();
        }

        /// <summary>
        /// Deleta um livro.
        /// </summary>
        /// <param name="id">ID do livro a ser deletado.</param>
        /// <returns>Status da operação.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            await _bookService.DeleteBookAsync(id);
            return NoContent();
        }
    }