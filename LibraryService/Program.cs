using System.Text.Json.Serialization;
using LibraryService.Api.Authors;
using LibraryService.Api.Books;
using LibraryService.Api.Works;
using LibraryService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace LibraryService
{
    public class Program
    {
        private const string CorsPolicyName = "_CorsPolicy";

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddControllers().AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<LibraryDbContext>(
                options => options
                .UseSqlServer(connectionString));

            builder.Services.AddScoped<IWorkService, WorkService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IAuthorService, AuthorService>();

            builder.Services.AddCors(options => options
                .AddPolicy(CorsPolicyName, policy =>
                    policy
                        .WithOrigins(new[] { builder.Configuration.GetValue<string>("Cors") })
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        ));


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(CorsPolicyName);
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}