using System;
using System.Collections.Generic;
using Warehouse.BLL.Interfaces;
using Warehouse.DAL.Repositories;
using Warehouse.Models;

namespace Warehouse.BLL.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly SupplierRepository _supplierRepo;

        // Sửa lỗi ở đây: Nhận Repo từ DI container thay vì tự 'new'
        public SupplierService(SupplierRepository supplierRepo)
        {
            _supplierRepo = supplierRepo;
        }

        // 1. Lấy danh sách NCC đang hoạt động
        public List<Supplier> GetAllActive()
        {
            try
            {
                return _supplierRepo.GetAll();
            }
            catch (Exception)
            {
                return new List<Supplier>();
            }
        }

        // 2. Logic thêm mới Nhà cung cấp
        public string AddSupplier(Supplier s)
        {
            // Kiểm tra tên NCC không được để trống
            if (string.IsNullOrWhiteSpace(s.SupplierName))
                return "Tên nhà cung cấp bắt buộc phải nhập.";

            // Kiểm tra độ dài số điện thoại (ví dụ)
            if (!string.IsNullOrEmpty(s.Phone) && s.Phone.Length < 10)
                return "Số điện thoại phải có ít nhất 10 chữ số.";

            try
            {
                bool result = _supplierRepo.Insert(s);
                return result ? "Thêm nhà cung cấp thành công!" : "Không thể thêm dữ liệu.";
            }
            catch (Exception ex)
            {
                return "Lỗi hệ thống: " + ex.Message;
            }
        }

        // 3. Logic Xóa mềm
        public bool DeleteSupplier(int id)
        {
            if (id <= 0) return false;

            try
            {
                // Có thể thêm kiểm tra tại đây: NCC này có đang có đơn hàng nào chưa hoàn tất không?
                // Nếu có thì không cho xóa.
                return _supplierRepo.SoftDelete(id);
            }
            catch (Exception)
            {
                return false;
            }
        }

        // 4. Logic Cập nhật (Nếu bạn đã có Repository Update tương ứng)
        public string UpdateSupplier(Supplier s)
        {
            if (s.SupplierId <= 0) return "ID không hợp lệ.";
            // Gọi Repo update...
            return "Tính năng đang cập nhật";
        }
    }
}