using Library_Management_System.Models;
using Library_Management_System.Repositories;


namespace Library_Management_System.Services.implementations;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<IEnumerable<Member>> GetAllMembersAsync()
    {
        return await _memberRepository.GetAllAsync();
    }

    public async Task<Library_Management_System.Models.Member> GetMemberByIdAsync(int id)
    {
       return await  _memberRepository.GetByIdAsync(id);
    }

    public async Task<Library_Management_System.Models.Member> CreateMemberAsync(Library_Management_System.Models.Member member)
    {
        await _memberRepository.AddAsync(member);
        await _memberRepository.SaveChangesAsync();
        return member;
    }

    public async Task UpdateMemberAsync(Library_Management_System.Models.Member member)
    {
        _memberRepository.Update(member);
        await _memberRepository.SaveChangesAsync();
    }

    public async Task DeleteMemberAsync(int id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member != null)
        {
            _memberRepository.Delete(member);
            await _memberRepository.SaveChangesAsync();
        }
    }
}