using Library_Management_System.Models;

namespace Library_Management_System.DTOs;

public class MemberDto
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }

    // Relacionamento 1:N com Loan
    public ICollection<LoanDto> Loans { get; set; }
}