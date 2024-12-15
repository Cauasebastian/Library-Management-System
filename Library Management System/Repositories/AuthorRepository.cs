using Library_Management_System.Data;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Repositories;

public interface IAuthorRepository : IRepository<Author>
{
    // Métodos específicos para Author, se necessário
}

public class AuthorRepository : IAuthorRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Author> _dbSet;

    public AuthorRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Author>();
    }

    public async Task<IEnumerable<Author>> GetAllAsync()
    {
        return await _dbSet.Include(a => a.Books).ToListAsync();
    }

    public async Task<Author> GetByIdAsync(int id)
    {
        return await _dbSet.Include(a => a.Books)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(Author entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(Author entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(Author entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}