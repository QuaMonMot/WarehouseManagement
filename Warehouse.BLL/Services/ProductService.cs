using System;
using System.Collections.Generic;
using Warehouse.BLL.Interfaces;
using Warehouse.DAL.Repositories;
using Warehouse.Models;

namespace Warehouse.BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductRepository _productRepo;

        // Sửa lỗi ở đây: Nhận Repo từ DI container thay vì tự 'new'
        public ProductService(ProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        public List<Product> GetProducts()
        {
            try
            {
                return _productRepo.GetAll();
            }
            catch
            {
                return new List<Product>();
            }
        }

        public string AddProduct(Product p)
        {
            try
            {
                bool success = _productRepo.Insert(p);
                return success ? "Thành công" : "Lỗi: Repository trả về false (kiểm tra lại logic DAL).";
            }
            catch (Exception ex)
            {
                // Dòng này cực kỳ quan trọng, nó sẽ hiện lỗi như "Login failed" hoặc "Table not found"
                return "Lỗi hệ thống chi tiết: " + ex.Message;
            }
        }
    }
}