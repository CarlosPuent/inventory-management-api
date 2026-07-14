namespace InventoryApi.Models
{
    
public record Supplier(int Id, string Name, string ContactEmail, bool IsActive)
    {
        public string GetStatusLabel()
        {

            return IsActive switch
            {
                true => "Proveedor Activo",
                false => "Proveedor Inactivo"
            };

        }
    }
}
