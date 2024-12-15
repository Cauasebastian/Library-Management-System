using Library_Management_System.Models;
using Library_Management_System.Repositories;

namespace Library_Management_System.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorService(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    public async Task<IEnumerable<Author>> GetAllAuthorsAsync()
    {
        return await _authorRepository.GetAllAsync();
    }

    public async Task<Author> GetAuthorByIdAsync(int id)
    {
        return await _authorRepository.GetByIdAsync(id);
    }

    public async Task<Author> CreateAuthorAsync(Author author)
    {
        await _authorRepository.AddAsync(author);
        await _authorRepository.SaveChangesAsync();
        return author;
    }

    public async Task UpdateAuthorAsync(Author author)
    {
        _authorRepository.Update(author);
        await _authorRepository.SaveChangesAsync();
    }

    public async Task DeleteAuthorAsync(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author != null)
        {
            _authorRepository.Delete(author);
            await _authorRepository.SaveChangesAsync();
        }
    }
}