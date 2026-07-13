using InventoryApi.DTOs;
using InventoryApi.Exceptions;
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

        public async Task<PagedResult<Supplier>> GetFilteredSuppliersAsync(SupplierFilterDto filter)
        {
            if (filter.Page < 1)
            {
                throw new BusinessRuleException("El número de página debe ser mayor o igual a 1.");
            }

            if (filter.PageSize < 1 || filter.PageSize > 100)
            {
                throw new BusinessRuleException("El tamaño de página debe estar entre 1 y 100.");
            }

            return await _supplierRepository.GetFilteredAsync(filter);
        }

        public async Task<PagedResult<Supplier>> GetPagedSuppliersAsync(int page, int pageSize)
        {
            if (page < 1)
            {
                throw new BusinessRuleException("El número de página debe ser mayor o igual a 1.");
            }

            if (pageSize < 1 || pageSize > 100)
            {
                throw new BusinessRuleException("El tamaño de página debe estar entre 1 y 100.");
            }

            return await _supplierRepository.GetPagedAsync(page, pageSize);
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
                throw new BusinessRuleException("El nombre del proveedor no puede estar vacío.");
            }

            if (string.IsNullOrWhiteSpace(supplier.ContactEmail) || !supplier.ContactEmail.Contains("@"))
            {
                throw new BusinessRuleException("Debe proporcionar un correo electrónico válido que contenga '@'.");
            }
        }
    }
}