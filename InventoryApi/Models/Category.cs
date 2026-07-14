namespace InventoryApi.Models
{
    public record Category(int Id, string Name, string Description, bool IsActive)
    {
        public string GetStatusLabel()
        {
            return IsActive switch
            {
                true => "Categoría Activa",
                false => "Categoría Inactiva"
            };
        }
    }
}
