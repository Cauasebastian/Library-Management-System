using Library_Management_System.Data;
using Library_Management_System.Models;
using Library_Management_System.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace Library_Management_System.Repositories;

public interface ILoanRepository : IRepository<Loan>
{
    Task<IEnumerable<Loan>> GetActiveLoansAsync();
    
    Task<IEnumerable<Loan>> GetActiveLoansByBookIdAsync(int bookId);
}

public class LoanRepository : ILoanRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Loan> _dbSet;

    public LoanRepository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<Loan>();
    }

    public async Task<IEnumerable<Loan>> GetAllAsync()
    {
        return await _dbSet.Include(l => l.Book)
            .Include(l => l.Member)
            .ToListAsync();
    }

    public async Task<Loan> GetByIdAsync(int id)
    {
        return await _dbSet.Include(l => l.Book)
            .Include(l => l.Member)
            .FirstOrDefaultAsync(l => l.Id == id);
    }

    public async Task<IEnumerable<Loan>> GetActiveLoansAsync()
    {
        return await _dbSet.Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.ReturnDate == null)
            .ToListAsync();
    }

    public async Task<IEnumerable<Loan>> GetActiveLoansByBookIdAsync(int bookId)
    {
        return await _dbSet.Include(l => l.Book)
            .Include(l => l.Member)
            .Where(l => l.BookId == bookId && l.ReturnDate == null)
            .ToListAsync();
    }

    public async Task AddAsync(Loan entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Update(Loan entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(Loan entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}