using FluentValidation;

namespace Application.CurrencyConversion
{
    public class ConvertCurrencyCommand
    {
        public string FromCurrencyCode { get; set; } = null!;
        public string ToCurrencyCode { get; set; } = null!;
        public decimal Amount { get; set; }
    }

    public class ConvertCurrencyValidator : AbstractValidator<ConvertCurrencyCommand>
    {
        public ConvertCurrencyValidator()
        {
            RuleFor(x => x.FromCurrencyCode).NotEmpty().WithMessage("FromCurrencyCode is required.");
            RuleFor(x => x.ToCurrencyCode).NotEmpty().WithMessage("ToCurrencyCode is required.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than 0.");
        }
    }
}