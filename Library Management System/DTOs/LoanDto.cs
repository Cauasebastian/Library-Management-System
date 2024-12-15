namespace Library_Management_System.DTOs;

public class LoanDto
{
    public int Id { get; set; }

    // Relacionamento com Book
    public int BookId { get; set; }
    public string BookTitle { get; set; }

    // Relacionamento com Member
    public int MemberId { get; set; }
    public string MemberName { get; set; }

    public DateTime LoanDate { get; set; }
    public DateTime? ReturnDate { get; set; }
}