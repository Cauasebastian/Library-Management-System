using Library_Management_System.Data;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Repositories;

public interface IBookRepository : IRepository<Book>
{
    Task<IEnumerable<Book>> GetBooksWithDetailsAsync();
}

public class BookRepository : IBookRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Book> _dbSet;

    public BookRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Book>();
    }

    public async Task<IEnumerable<Book>> GetAllAsync()
    {
        return await GetBooksWithDetailsAsync();
    }

    public async Task<Book> GetByIdAsync(int id)
    {
        return await _dbSet.Include(b => b.Author)
            .Include(b => b.BookCategories)
            .ThenInclude(bc => bc.Category)
            .FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<IEnumerable<Book>> GetBooksWithDetailsAsync()
    {
        return await _dbSet.Include(b => b.Author)
            .Include(b => b.BookCategories)
            .ThenInclude(bc => bc.Category)
            .ToListAsync();
    }

    public async Task AddAsync(Book entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(Book entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(Book entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}