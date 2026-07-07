
namespace InventoryApi.Models
{
    public record AppUsers(int Id, string Name, string LastName, string ContactEmail, string phoneNumber, int Age, double Weight, bool IsActive)
    {

        public string GetStatusLabel()
        {
            return IsActive switch
            {
                true => "Usuario Activo",
                false => "Usuario Inactivo"
            };
        }

        public async Task<string> ValidateEmailAsync()
        {
            await Task.Delay(1000);
            return $"Email validado: {ContactEmail}";
        }

    }
}
