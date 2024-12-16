using Library_Management_System.Data;
using Library_Management_System.Repositories;
using Library_Management_System.Services;
using Library_Management_System.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using FluentValidation.AspNetCore;
using Library_Management_System.Services.implementations;

var builder = WebApplication.CreateBuilder(args);

// Configure MySQL (change connection string to match your MySQL setup)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("LibraryDbConnection"),
    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("LibraryDbConnection"))));

// Register repositories
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();

// Register services
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<ILoanService, LoanService>();

// AutoMapper
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

// Controllers with FluentValidation
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>());

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "LibraryManagementSystem API", Version = "v1" });

    // Include XML comments (if needed)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

var app = builder.Build();

// Migrate database automatically at startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    // Apply any pending migrations to the database
    dbContext.Database.Migrate();
    
    // Initialize test data (optional)
    SeedData(dbContext);
}

// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Method to populate test data
void SeedData(ApplicationDbContext context)
{
    // Check if data already exists
    if (!context.Authors.Any())
    {
        var author1 = new Author { Name = "J.K. Rowling" };
        var author2 = new Author { Name = "George R.R. Martin" };

        context.Authors.AddRange(author1, author2);
        context.SaveChanges();

        var category1 = new Category { Name = "Fantasy" };
        var category2 = new Category { Name = "Adventure" };
        var category3 = new Category { Name = "Drama" };

        context.Categories.AddRange(category1, category2, category3);
        context.SaveChanges();

        var book1 = new Book
        {
            Title = "Harry Potter and the Philosopher's Stone",
            AuthorId = author1.Id,
            BookCategories = new List<BookCategory>
            {
                new BookCategory { CategoryId = category1.Id },
                new BookCategory { CategoryId = category2.Id }
            }
        };

        var book2 = new Book
        {
            Title = "A Game of Thrones",
            AuthorId = author2.Id,
            BookCategories = new List<BookCategory>
            {
                new BookCategory { CategoryId = category1.Id },
                new BookCategory { CategoryId = category3.Id }
            }
        };

        context.Books.AddRange(book1, book2);
        context.SaveChanges();

        var member1 = new Member { FullName = "John Doe", Email = "john.doe@example.com" };
        var member2 = new Member { FullName = "Jane Smith", Email = "jane.smith@example.com" };

        context.Members.AddRange(member1, member2);
        context.SaveChanges();

        var loan1 = new Loan
        {
            BookId = book1.Id,
            MemberId = member1.Id,
            LoanDate = System.DateTime.UtcNow
        };

        context.Loans.Add(loan1);
        context.SaveChanges();
    }
}
