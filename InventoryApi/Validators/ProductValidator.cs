using FluentValidation;
using InventoryApi.Models;

namespace InventoryApi.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("El nombre del producto no puede estar vacío.")
            .MaximumLength(100).WithMessage("El nombre no puede superar los 100 caracteres.");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("El precio del producto debe ser mayor a cero.");

        RuleFor(p => p.Stock)
            .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");

        RuleFor(p => p.CategoryId)
            .GreaterThan(0).WithMessage("Debe especificar una categoría válida.");
    }
}