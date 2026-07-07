using InventoryApi.Models;
using InventoryApi.Repositories;

namespace InventoryApi.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly List<Category> _categories = new()
        {
            new Category(1, "Electrónicos", "Dispositivos principales como laptops, tablets y smartphones", true),
            new Category(2, "Periféricos", "Accesorios de computadora como teclados mecánicos, ratones y monitores", true),
            new Category(3, "Software", "Licencias de programas, sistemas operativos y suscripciones digitales", true),
            new Category(4, "Redes", "Equipamiento de conectividad incluyendo routers, switches y cableado estructurado", true),
            new Category(5, "Mobiliario de Oficina", "Sillas ergonómicas, escritorios y soportes para equipo técnico", false)
        };


        public Task<List<Category>> GetAllAsync()
        {
            return Task.FromResult(_categories);
        }

        public Task<Category?> GetByIdAsync(int id)
        {
            var category = _categories.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(category);
        }

        public Task<Category> AddAsync(Category category)
        {
            var nextId = _categories.Count == 0 ? 1 : _categories.Max(c => c.Id) + 1;
            var created = category with { Id = nextId };
            return Task.FromResult(created);
        }
    }
}
