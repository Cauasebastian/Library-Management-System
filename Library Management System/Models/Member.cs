using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models;

public class Member
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string FullName { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    // Relação 1:N com Loan
    public ICollection<Loan> Loans { get; set; }
}