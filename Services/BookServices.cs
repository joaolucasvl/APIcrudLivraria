using FluentValidation;
using gerenciadorLivraria.DTOs;
using gerenciadorLivraria.Infrastructure.Data;
using gerenciadorLivraria.Models;
using gerenciadorLivraria.Enums;
using Microsoft.EntityFrameworkCore;



namespace gerenciadorLivraria.Services;

public class BookServices
{
    private readonly AppDbContext _context;
    private readonly IValidator<CreateBookDto> _validator;
    private readonly IValidator<UpdateBookDto> _updateValidator;

    public BookServices(AppDbContext context, IValidator<CreateBookDto> validator, IValidator<UpdateBookDto> updateValidator)
    {
        _context = context;
        _validator = validator;
        _updateValidator = updateValidator;
    }

    public async Task<Book> CreateAsync(CreateBookDto dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);
        if (!validationResult.IsValid) 
            throw new ValidationException(validationResult.Errors);

        var title = dto.Title.Trim();
        var author = dto.Author.Trim();

        var alreadyExists = await _context.Books
            .AnyAsync(b =>
            b.Title == title &&
            b.Author == author
         );

        if (alreadyExists)
            throw new InvalidOperationException("Livro já Cadastrado.");



        var genre = Enum.Parse<Genre>(dto.Genre, true);

        var book = new Book(
            dto.Title,
            dto.Author,
            genre,
            dto.Price,
            dto.Stock
        );

        _context.Books.Add( book );
        await _context.SaveChangesAsync();

        return book;
    }

    public async Task<List<BookResponseDto>> GetBooksAsync()
    {
        return await _context.Books
            .AsNoTracking()
            .Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre.ToString(),
                Price = b.Price,
                Stock = b.Stock,

            })
            .ToListAsync();
    }


    public async Task<BookResponseDto?> GetByIdAsync(Guid id)
    {
        return await _context.Books
            .AsNoTracking()
            .Where(b => b.Id == id)
            .Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre.ToString(),
                Price = b.Price,
                Stock = b.Stock,

            })
            .FirstOrDefaultAsync();
    }

    public async Task<Book?> UpdateAsync(Guid id, UpdateBookDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var validationResult = await _updateValidator.ValidateAsync(dto);
           if (!validationResult.IsValid)
              throw new ValidationException(validationResult.Errors);

        var book = await _context.Books
             .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
            return null;

        var title = dto.Title.Trim();
        var author = dto.Author.Trim();

        // Evitar duplicidade (outro livro com mesmo Title + Author)
        var duplicateExists = await _context.Books
            .AnyAsync(b =>
                b.Id != id &&
                b.Title == title &&
                b.Author == author
            );

        if (duplicateExists)
            throw new InvalidOperationException("Já existe outro livro com esse título e autor.");

        var genre = Enum.Parse<Genre>(dto.Genre, true);

        book.Update(
            title,
            author,
            genre,
            dto.Price,
            dto.Stock
        );

        await _context.SaveChangesAsync();

        return book;

    }

    public async Task<Book> DeleteAsync(Guid id)
    {
        var book = await _context.Books
            .FirstOrDefaultAsync(b => b.Id == id);

        if (book == null)
            return null;

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return book;
    }
}
