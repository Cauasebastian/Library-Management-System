namespace Library_Management_System.DTOs;

public class AuthorDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public ICollection<BookDto> Books { get; set; }
}