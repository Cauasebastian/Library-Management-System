using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models;

public class Author
{
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    // Relação 1:N com Book
    public ICollection<Book> Books { get; set; }
}