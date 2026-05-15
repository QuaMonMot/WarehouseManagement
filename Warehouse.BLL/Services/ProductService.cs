using Warehouse.BLL.Interfaces;
using Warehouse.DAL.Interfaces;
using Warehouse.Models;

namespace Warehouse.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Product id must be greater than 0");
            }

            return _productRepository.GetById(id);
        }

        public void Add(Product product)
        {
            ValidateProduct(product);

            _productRepository.Add(product);
        }

        public void Update(Product product)
        {
            if (product.ProductId <= 0)
            {
                throw new ArgumentException("Product id must be greater than 0");
            }

            ValidateProduct(product);

            _productRepository.Update(product);
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("Product id must be greater than 0");
            }

            _productRepository.Delete(id);
        }

        private static void ValidateProduct(Product product)
        {
            if (string.IsNullOrWhiteSpace(product.SKU))
            {
                throw new ArgumentException("SKU is required");
            }

            if (string.IsNullOrWhiteSpace(product.ProductName))
            {
                throw new ArgumentException("Product name is required");
            }

            if (product.Quantity < 0)
            {
                throw new ArgumentException("Quantity cannot be negative");
            }

            if (product.Price < 0)
            {
                throw new ArgumentException("Price cannot be negative");
            }

            if (product.MinStock < 0)
            {
                throw new ArgumentException("Minimum stock cannot be negative");
            }

            if (product.SupplierId <= 0)
            {
                throw new ArgumentException("Supplier id must be greater than 0");
            }
        }
    }
}
