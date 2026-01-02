using FluentValidation;
using Domain.Entities;

namespace Application.Currencies.Commands
{
    public class CreateCurrencyCommand
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public decimal RateToBase { get; set; }
    }

    public class CreateCurrencyValidator : AbstractValidator<CreateCurrencyCommand>
    {
        public CreateCurrencyValidator()
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("Currency code is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Currency name is required.");
            RuleFor(x => x.RateToBase).GreaterThan(0).WithMessage("RateToBase must be greater than 0.");
        }
    }
}