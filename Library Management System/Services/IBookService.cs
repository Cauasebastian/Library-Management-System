using Library_Management_System.Models;

namespace Library_Management_System.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllBooksAsync();

    Task<Book> GetBookByIdAsync(int id);

    Task<Book> CreateBookAsync(Book book);

    Task UpdateBookAsync(Book book);

    Task DeleteBookAsync(int id);
}