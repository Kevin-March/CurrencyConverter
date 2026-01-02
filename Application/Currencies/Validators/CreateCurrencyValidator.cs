using FluentValidation;

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

// Validator para la conversión de divisas
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