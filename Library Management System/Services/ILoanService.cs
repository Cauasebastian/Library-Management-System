using Library_Management_System.Models;

namespace Library_Management_System.Services;

public interface ILoanService
{
    Task<IEnumerable<Loan>> GetAllLoansAsync();
    Task<Loan> GetLoanByIdAsync(int id);
    Task<Loan> CreateLoanAsync(Loan loan);
    Task UpdateLoanAsync(Loan loan);
    Task DeleteLoanAsync(int id);
    
    Task<IEnumerable<Loan>> GetActiveLoansByBookIdAsync(int bookId);
}