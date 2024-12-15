using Library_Management_System.Data;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Repositories;

public interface ICategoryRepository : IRepository<Category>
{
    // Métodos específicos para Category, se necessário
}

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Category> _dbSet;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Category>();
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _dbSet.Include(c => c.BookCategories)
            .ThenInclude(bc => bc.Book)
            .ToListAsync();
    }

    public async Task<Category> GetByIdAsync(int id)
    {
        return await _dbSet.Include(c => c.BookCategories)
            .ThenInclude(bc => bc.Book)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Category entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(Category entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(Category entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}