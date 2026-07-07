using InventoryApi.Models;

private void ValidateAppUserBusinessRules(AppUser appUser)
{
    if (string.IsNullOrWhiteSpace(appUser.Name))
    {
        throw new ArgumentException("El nombre del usuario no puede estar vacío.");
    }

    if (string.IsNullOrWhiteSpace(appUser.LastName))
    {
        throw new ArgumentException("El apellido del usuario no puede estar vacío.");
    }

    if (string.IsNullOrWhiteSpace(appUser.ContactEmail) || !appUser.ContactEmail.Contains("@"))
    {
        throw new ArgumentException("Debe proporcionar un correo electrónico válido que contenga '@'.");
    }

    if (string.IsNullOrWhiteSpace(appUser.PhoneNumber) || appUser.PhoneNumber.Length < 8)
    {
        throw new ArgumentException("Debe proporcionar un número de teléfono válido de al menos 8 dígitos.");
    }

    if (appUser.Age <= 0 || appUser.Age > 120)
    {
        throw new ArgumentException("La edad debe estar entre 1 y 120 años.");
    }

    if (appUser.Weight <= 0)
    {
        throw new ArgumentException("El peso debe ser mayor a cero.");
    }
}