using FluentValidation;
using InventoryApi.DTOs;

namespace InventoryApi.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequestDto>
{
    public RegisterRequestValidator()
    {
        RuleFor(r => r.Name)
            .NotEmpty().WithMessage("El nombre no puede estar vacío.");

        RuleFor(r => r.LastName)
            .NotEmpty().WithMessage("El apellido no puede estar vacío.");

        RuleFor(r => r.ContactEmail)
            .NotEmpty().WithMessage("Debe proporcionar un correo electrónico.")
            .EmailAddress().WithMessage("Debe proporcionar un correo electrónico con formato válido.");

        RuleFor(r => r.PhoneNumber)
            .NotEmpty().WithMessage("Debe proporcionar un número de teléfono.")
            .MinimumLength(8).WithMessage("El número de teléfono debe tener al menos 8 dígitos.");

        RuleFor(r => r.Age)
            .InclusiveBetween(1, 120).WithMessage("La edad debe estar entre 1 y 120 años.");

        RuleFor(r => r.Weight)
            .GreaterThan(0).WithMessage("El peso debe ser mayor a cero.");

        RuleFor(r => r.Password)
            .NotEmpty().WithMessage("Debe proporcionar una contraseña.")
            .MinimumLength(8).WithMessage("La contraseña debe tener al menos 8 caracteres.");
    }
}
