using LibraryService.Api.Authors;
using LibraryService.Api.Books;
using LibraryService.Api.Works;
using LibraryService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryService
{
    public partial class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<LibraryDbContext>(
                options => options
                .UseSqlServer(connectionString));

            builder.Services.AddScoped<WorkService>();
            builder.Services.AddScoped<BookService>();
            builder.Services.AddScoped<IAuthorService, AuthorService>();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}