using System.Collections;
using System.Xml.Linq;

namespace InventoryApi.Models
{
    public record Product(int Id, string Name, decimal Price, int Stock, int CategoryId)
    {
        public Category? Category { get; init; }

        public string GetStockStatus()
        {
            return Stock switch
            {
                0 => "Agotado",
                <= 10 => "Bajo",
                _ => "Disponible"
            };
        }

        public async Task<Product> SimulateDatabaseFetchAsync()
        {
            await Task.Delay(1000);
            return this;
        }
    }
}
