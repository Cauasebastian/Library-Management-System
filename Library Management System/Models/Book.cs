using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models;

public class Book
{
    public int Id { get; set; }

    [Required]
    [MaxLength(200)]
    public string Title { get; set; }

    public int AuthorId { get; set; }
    public Author Author { get; set; }

    // Relação N:M com Category
    public ICollection<BookCategory> BookCategories { get; set; }

    // Relação 1:N com Loan
    public ICollection<Loan> Loans { get; set; }
}