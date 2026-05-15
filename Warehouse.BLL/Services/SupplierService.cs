using Warehouse.BLL.Interfaces;
using Warehouse.DAL.Interfaces;
using Warehouse.Models;

namespace Warehouse.BLL.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(
            ISupplierRepository supplierRepository
        )
        {
            _supplierRepository = supplierRepository;
        }

        public List<Supplier> GetAll()
        {
            return _supplierRepository.GetAll();
        }

        public Supplier GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Supplier id must be greater than 0");
            }

            return _supplierRepository.GetById(id);
        }

        public void Add(Supplier supplier)
        {
            ValidateSupplier(supplier);

            _supplierRepository.Add(supplier);
        }

        public void Update(Supplier supplier)
        {
            if (supplier.SupplierId <= 0)
            {
                throw new ArgumentException("Supplier id must be greater than 0");
            }

            ValidateSupplier(supplier);

            _supplierRepository.Update(supplier);
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Supplier id must be greater than 0");
            }

            _supplierRepository.Delete(id);
        }

        private static void ValidateSupplier(Supplier supplier)
        {
            if (string.IsNullOrWhiteSpace(supplier.SupplierCode))
            {
                throw new ArgumentException("Supplier code is required");
            }

            if (string.IsNullOrWhiteSpace(supplier.SupplierName))
            {
                throw new ArgumentException("Supplier name is required");
            }
        }
    }
}
