using Library_Management_System.Models;
using Library_Management_System.Repositories;

namespace Library_Management_System.Services;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    
    public LoanService(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }
    
    public async Task<IEnumerable<Loan>> GetAllLoansAsync()
    {
        return await _loanRepository.GetAllAsync();
    }

    public async Task<Loan> GetLoanByIdAsync(int id)
    {
       return await _loanRepository.GetByIdAsync(id);
    }

    public async Task<Loan> CreateLoanAsync(Loan loan)
    {
       await _loanRepository.AddAsync(loan);
         await _loanRepository.SaveChangesAsync();
            return loan;
    }

    public async Task UpdateLoanAsync(Loan loan)
    {
         _loanRepository.Update(loan);
        await _loanRepository.SaveChangesAsync();
    }

    public async Task DeleteLoanAsync(int id)
    {
        var loan = await _loanRepository.GetByIdAsync(id);
        if (loan != null)
        {
            _loanRepository.Delete(loan);
            await _loanRepository.SaveChangesAsync();
        }
        else
        {
            throw new Exception("Loan not found");
        }
    }
}