using AutoMapper;
using Library_Management_System.DTOs;
using Library_Management_System.Models;
using Library_Management_System.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library_Management_System.Controllers;

[Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorsController> _logger;

        public AuthorsController(IAuthorService authorService, IMapper mapper, ILogger<AuthorsController> logger)
        {
            _authorService = authorService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Obtém todos os autores.
        /// </summary>
        /// <returns>Lista de autores.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AuthorDto>>> GetAuthors()
        {
            var authors = await _authorService.GetAllAuthorsAsync();
            var authorsDto = _mapper.Map<IEnumerable<AuthorDto>>(authors);
            return Ok(authorsDto);
        }

        /// <summary>
        /// Obtém um autor pelo ID.
        /// </summary>
        /// <param name="id">ID do autor.</param>
        /// <returns>Autor correspondente ao ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<AuthorDto>> GetAuthor(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                _logger.LogWarning("Autor com ID {Id} não encontrado.", id);
                return NotFound();
            }

            var authorDto = _mapper.Map<AuthorDto>(author);
            return Ok(authorDto);
        }

        /// <summary>
        /// Cria um novo autor.
        /// </summary>
        /// <param name="authorDto">Dados do autor a ser criado.</param>
        /// <returns>Autor criado.</returns>
        [HttpPost]
        public async Task<ActionResult<AuthorDto>> CreateAuthor([FromBody] AuthorDto authorDto)
        {
            var author = _mapper.Map<Author>(authorDto);
            await _authorService.CreateAuthorAsync(author);
            var createdAuthorDto = _mapper.Map<AuthorDto>(author);

            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, createdAuthorDto);
        }

        /// <summary>
        /// Atualiza um autor existente.
        /// </summary>
        /// <param name="id">ID do autor a ser atualizado.</param>
        /// <param name="authorDto">Dados atualizados do autor.</param>
        /// <returns>Status da operação.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorDto authorDto)
        {
            if (id != authorDto.Id)
            {
                return BadRequest("ID do autor na URL não corresponde ao ID no corpo.");
            }

            var existingAuthor = await _authorService.GetAuthorByIdAsync(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            _mapper.Map(authorDto, existingAuthor);
            await _authorService.UpdateAuthorAsync(existingAuthor);

            return NoContent();
        }

        /// <summary>
        /// Deleta um autor.
        /// </summary>
        /// <param name="id">ID do autor a ser deletado.</param>
        /// <returns>Status da operação.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            var author = await _authorService.GetAuthorByIdAsync(id);
            if (author == null)
            {
                return NotFound();
            }

            await _authorService.DeleteAuthorAsync(id);
            return NoContent();
        }
}
