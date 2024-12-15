using Library_Management_System.Models;
using Library_Management_System.Repositories;

namespace Library_Management_System.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _bookRepository.GetAllAsync();
    }

    public async Task<Book> GetBookByIdAsync(int id)
    {
        return await _bookRepository.GetByIdAsync(id);
    }

    public async Task<Book> CreateBookAsync(Book book)
    {
        await _bookRepository.AddAsync(book);
        await _bookRepository.SaveChangesAsync();
        return book;
    }

    public async Task UpdateBookAsync(Book book)
    {
        _bookRepository.Update(book);
        await _bookRepository.SaveChangesAsync();
    }

    public async Task DeleteBookAsync(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book != null)
        {
            _bookRepository.Delete(book);
            await _bookRepository.SaveChangesAsync();
        }
    }
}