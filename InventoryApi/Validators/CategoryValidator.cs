using FluentValidation;
using InventoryApi.Models;

namespace InventoryApi.Validators;

public class CategoryValidator : AbstractValidator<Category>
{
    public CategoryValidator()
    {
        RuleFor(c => c.Name)
            .NotEmpty().WithMessage("El nombre de la categoría no puede estar vacío.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(c => c.Description)
            .NotEmpty().WithMessage("La descripción de la categoría es obligatoria.")
            .Length(5, 100).WithMessage("La descripción debe tener entre 5 y 100 caracteres.");
    }
}