using FluentValidation;
using gerenciadorLivraria.DTOs;
using gerenciadorLivraria.Infrastructure.Data;
using gerenciadorLivraria.Services;
using gerenciadorLivraria.Validators;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookValidator>();
builder.Services.AddScoped<IValidator<UpdateBookDto>, UpdateBookValidator>();
builder.Services.AddScoped<BookServices>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
