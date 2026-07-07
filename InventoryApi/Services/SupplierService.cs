using InventoryApi.Models;
using InventoryApi.Repositories;

namespace InventoryApi.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public Task<List<Supplier>> GetAllSuppliersAsync()
        {
            return _supplierRepository.GetAllAsync();
        }

        public Task<Supplier?> GetSupplierByIdAsync(int id)
        {
            return _supplierRepository.GetByIdAsync(id);
        }

        public Task<Supplier> CreateSupplierAsync(Supplier supplier)
        {
            ValidateSupplierBusinessRules(supplier);
            return _supplierRepository.AddAsync(supplier);
        }

        public async Task<Supplier?> UpdateSupplierAsync(int id, Supplier supplier)
        {
            ValidateSupplierBusinessRules(supplier);
            return await _supplierRepository.UpdateAsync(id, supplier);
        }

        public async Task<bool> DeleteSupplierAsync(int id)
        {
            return await _supplierRepository.DeleteAsync(id);
        }

        private void ValidateSupplierBusinessRules(Supplier supplier)
        {
            if (string.IsNullOrWhiteSpace(supplier.Name))
            {
                throw new ArgumentException("El nombre del proveedor no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(supplier.ContactEmail) || !supplier.ContactEmail.Contains("@"))
            {
                throw new ArgumentException("Debe proporcionar un correo electrónico válido que contenga '@'.");
            }
        }
    }
}