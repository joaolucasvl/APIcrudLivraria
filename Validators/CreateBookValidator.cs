using FluentValidation;
using gerenciadorLivraria.DTOs;



namespace gerenciadorLivraria.Validators;

public class CreateBookValidator : AbstractValidator<CreateBookDto>
{
    public CreateBookValidator() 
    { 
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(2, 120);

        RuleFor(x => x.Author) 
            .NotEmpty()
            .Length(2, 120);

        RuleFor(x => x.Genre)
            .Must(BeValidGenre)
            .WithMessage("Gênero Invalido");

        RuleFor(x => x.Price)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Stock)
            .NotEmpty()
            .GreaterThanOrEqualTo(0);

    }

    private bool BeValidGenre(string genre)
    {
        return Enum.TryParse<Enums.Genre>(genre, true, out _);
    }
}
