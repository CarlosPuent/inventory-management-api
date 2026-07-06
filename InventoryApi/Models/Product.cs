using System.Collections;
using System.Xml.Linq;

namespace InventoryApi.Models
{
   
    public record Product(int Id, String Name, decimal Price, int Stock)
    {

        public string GetStockStatus()
        {
            return Stock switch
            {
                0 => "Agotado",
                <= 10 => "Bajo",
                _=> "Disponible"
            };
        }

        public async Task<Product> SimulateDatabaseFetchAsync()
        {
            await Task.Delay(1000);
            return this;
        }
    }

    public record  Category(int Id, string Name)
    {

        public async Task<string> GetDescriptionAsync()
        {
            await Task.Delay(500); ///simulando una consulta lenta a la BD
            return $"Categoria: {Name}";
         //   `Categoría: ${ name}` JavaScript
         //   String.format()   Java
        }

    }
}
