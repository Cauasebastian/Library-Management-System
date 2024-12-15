using System.ComponentModel.DataAnnotations;

namespace Library_Management_System.Models;

public class Category
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }

    // Relação N:M com Book através da tabela de junção BookCategory
    public ICollection<BookCategory> BookCategories { get; set; }
}

public class BookCategory
{
    public int BookId { get; set; }
    public Book Book { get; set; }

    public int CategoryId { get; set; }
    public Category Category { get; set; }
}