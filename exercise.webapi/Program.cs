using exercise.webapi.Data;
using exercise.webapi.Endpoints;
using exercise.webapi.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<DataContext>(opt => opt.UseInMemoryDatabase("Library"));
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IAuthorRepository, AuthorRepository>();
builder.Services.AddDbContext<DataContext>();

var app = builder.Build();

//using (var dbContext = new DataContext(new DbContextOptions<DataContext>()))
//{
//    dbContext.Database.EnsureCreated();
//}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.ConfigureBooksEndpoints();
app.ConfigureAuthorsEndpoints();
app.Run();
