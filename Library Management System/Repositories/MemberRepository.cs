using Library_Management_System.Data;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Repositories;

public interface IMemberRepository : IRepository<Member>
{
    // Métodos específicos para Member, se necessário
}

public class MemberRepository : IMemberRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Member> _dbSet;

    public MemberRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Member>();
    }

    public async Task<IEnumerable<Member>> GetAllAsync()
    {
        return await _dbSet.Include(m => m.Loans)
            .ThenInclude(l => l.Book)
            .ToListAsync();
    }

    public async Task<Member> GetByIdAsync(int id)
    {
        return await _dbSet.Include(m => m.Loans)
            .ThenInclude(l => l.Book)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddAsync(Member entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(Member entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(Member entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}