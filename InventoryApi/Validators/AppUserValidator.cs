using FluentValidation;
using InventoryApi.Models;

namespace InventoryApi.Validators;

public class AppUserValidator : AbstractValidator<AppUser>
{
    public AppUserValidator()
    {
        RuleFor(u => u.Name)
            .NotEmpty().WithMessage("El nombre del usuario no puede estar vacío.");

        RuleFor(u => u.LastName)
            .NotEmpty().WithMessage("El apellido del usuario no puede estar vacío.");

        RuleFor(u => u.ContactEmail)
            .NotEmpty().WithMessage("Debe proporcionar un correo electrónico.")
            .EmailAddress().WithMessage("Debe proporcionar un correo electrónico con formato válido.");

        RuleFor(u => u.PhoneNumber)
            .NotEmpty().WithMessage("Debe proporcionar un número de teléfono.")
            .MinimumLength(8).WithMessage("El número de teléfono debe tener al menos 8 dígitos.");

        RuleFor(u => u.Age)
            .InclusiveBetween(1, 120).WithMessage("La edad debe estar entre 1 y 120 años.");

        RuleFor(u => u.Weight)
            .GreaterThan(0).WithMessage("El peso debe ser mayor a cero.");
    }
}