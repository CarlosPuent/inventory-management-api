using InventoryApi.Models;

namespace InventoryApi.Repositories
{
    public class SupplierRepository : ISupplierRepository
    {
        private readonly List<Supplier> _suppliers = new()
        {
            new Supplier(1, "Distribuidora Tech", "contacto@distribuidoratech.com", true),
            new Supplier(2, "Suministros Globales", "ventas@suministrosglobales.sv", true),
            new Supplier(3, "Importaciones Rápidas", "info@importacionesrapidas.com", false)
        };

        public Task<List<Supplier>> GetAllAsync()
        {
            return Task.FromResult(_suppliers);
        }

        public Task<Supplier?> GetByIdAsync(int id)
        {
            var supplier = _suppliers.FirstOrDefault(s => s.Id == id);
            return Task.FromResult(supplier);
        }

        public Task<Supplier> AddAsync(Supplier supplier)
        {
            _suppliers.Add(supplier);
            return Task.FromResult(supplier);
        }
    }
}