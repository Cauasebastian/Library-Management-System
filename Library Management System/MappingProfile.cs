using AutoMapper;
using Library_Management_System.DTOs;
using Library_Management_System.Models;

namespace Library_Management_System;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Author Mapping
        CreateMap<Author, AuthorDto>()
            .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));
        CreateMap<AuthorDto, Author>();

        // Category Mapping
        CreateMap<Category, CategoryDto>()
            .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.BookCategories.Select(bc => bc.Book)));
        CreateMap<CategoryDto, Category>();

        // Book Mapping
        CreateMap<Book, BookDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
            .ForMember(dest => dest.CategoryNames, opt => opt.MapFrom(src => src.BookCategories.Select(bc => bc.Category.Name)))
            .ForMember(dest => dest.Loans, opt => opt.MapFrom(src => src.Loans));
        CreateMap<BookDto, Book>()
            .ForMember(dest => dest.BookCategories, opt => opt.Ignore())
            .ForMember(dest => dest.Loans, opt => opt.Ignore());

        // Member Mapping
        CreateMap<Member, MemberDto>()
            .ForMember(dest => dest.Loans, opt => opt.MapFrom(src => src.Loans));
        CreateMap<MemberDto, Member>();

        // Loan Mapping
        CreateMap<Loan, LoanDto>()
            .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
            .ForMember(dest => dest.MemberName, opt => opt.MapFrom(src => src.Member.FullName));
        CreateMap<LoanDto, Loan>()
            .ForMember(dest => dest.Book, opt => opt.Ignore())
            .ForMember(dest => dest.Member, opt => opt.Ignore());
    }
}