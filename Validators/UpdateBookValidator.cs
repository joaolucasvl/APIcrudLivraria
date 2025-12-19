using FluentValidation;
using gerenciadorLivraria.DTOs;
using gerenciadorLivraria.Enums;

namespace gerenciadorLivraria.Validators;

public class UpdateBookValidator : AbstractValidator<UpdateBookDto>
{
        public UpdateBookValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .Length(2, 120);

            RuleFor(x => x.Author)
                .NotEmpty()
                .Length(2, 120);

            RuleFor(x => x.Genre)
                .Must(BeValidGenre)
                .WithMessage("Gênero inválido");

            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0);
        }

        private bool BeValidGenre(string genre)
        {
            return Enum.TryParse<Genre>(genre, true, out _);
        }

}
