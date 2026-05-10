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
            // Kiểm tra nghiệp vụ
            if (string.IsNullOrWhiteSpace(p.ProductName))
                return "Tên sản phẩm không được để trống.";
            if (p.Price < 0)
                return "Giá sản phẩm không được là số âm.";
            if (p.CategoryId <= 0)
                return "Vui lòng chọn danh mục hợp lệ.";

            try
            {
                bool success = _productRepo.Insert(p);
                return success ? "Thêm sản phẩm và khởi tạo kho thành công!" : "Lỗi khi thêm sản phẩm.";
            }
            catch (Exception ex)
            {
                return "Lỗi hệ thống: " + ex.Message;
            }
        }
    }
}