using Library_Management_System.Models;

namespace Library_Management_System.DTOs;

public class BookDto
{
    public int Id { get; set; }
    public string Title { get; set; }

    // Relacionamento com Author
    public int AuthorId { get; set; }
    public string AuthorName { get; set; }

    // Relacionamento N:M com Category
    public ICollection<int> CategoryIds { get; set; }
    public ICollection<string> CategoryNames { get; set; }

    // Informações sobre empréstimos
    public ICollection<LoanDto> Loans { get; set; }
}