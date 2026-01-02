using FluentValidation;
using Domain.Entities;

namespace Application.Addresses.Commands
{
    public class CreateAddressCommand
    {
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string? ZipCode { get; set; }
    }

    public class CreateAddressValidator : AbstractValidator<CreateAddressCommand>
    {
        public CreateAddressValidator()
        {
            RuleFor(x => x.Street).NotEmpty().WithMessage("Street is required.");
            RuleFor(x => x.City).NotEmpty().WithMessage("City is required.");
            RuleFor(x => x.Country).NotEmpty().WithMessage("Country is required.");
        }
    }
}