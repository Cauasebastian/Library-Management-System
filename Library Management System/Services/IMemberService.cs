using Library_Management_System.Models;

namespace Library_Management_System.Services;

public interface IMemberService
{
    Task<IEnumerable<Member>> GetAllMembersAsync();
    Task<Member> GetMemberByIdAsync(int id);
    Task<Member> CreateMemberAsync(Member member);
    Task UpdateMemberAsync(Member member);
    Task DeleteMemberAsync(int id);
}