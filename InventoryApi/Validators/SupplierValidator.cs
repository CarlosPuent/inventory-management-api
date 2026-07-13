using FluentValidation;
using InventoryApi.Models;

namespace InventoryApi.Validators;

public class SupplierValidator : AbstractValidator<Supplier>
{
    public SupplierValidator()
    {
        RuleFor(s => s.Name)
            .NotEmpty().WithMessage("El nombre del proveedor no puede estar vacío.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(s => s.ContactEmail)
            .NotEmpty().WithMessage("Debe proporcionar un correo electrónico.")
            .EmailAddress().WithMessage("Debe proporcionar un correo electrónico con formato válido.");
    }
}