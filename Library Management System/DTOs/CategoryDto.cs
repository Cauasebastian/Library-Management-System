using Library_Management_System.Models;

namespace Library_Management_System.DTOs;

public class CategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<BookDto> Books { get; set; }
}
