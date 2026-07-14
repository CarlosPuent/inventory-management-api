using System.Text.Json.Serialization;
using InventoryApi.Models.Enums;

namespace InventoryApi.Models
{
    public record AppUser(
        int Id,
        string Name,
        string LastName,
        string ContactEmail,
        string PhoneNumber,
        int Age,
        double Weight,
        bool IsActive,
        [property: JsonIgnore] string PasswordHash = "",
        UserRole Role = UserRole.User
    )
    {
        public string GetStatusLabel()
        {
            return IsActive switch
            {
                true => "Usuario Activo",
                false => "Usuario Inactivo"
            };
        }
    }
}