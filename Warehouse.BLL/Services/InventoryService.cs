using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.BLL.Interfaces;
using Warehouse.DAL.Repositories;

namespace Warehouse.BLL.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly InventoryRepository _inventoryRepo;

        // Sửa lỗi ở đây: Nhận Repo từ DI container thay vì tự 'new'
        public InventoryService(InventoryRepository inventoryRepo)
        {
            _inventoryRepo = inventoryRepo;
        }

        // 1. Logic Nhập hàng
        public string ImportGoods(int supplierId, int userId, int productId, int qty, decimal price)
        {
            // Validation: Kiểm tra logic nghiệp vụ trước khi gọi database
            if (qty <= 0) return "Số lượng nhập phải lớn hơn 0.";
            if (price < 0) return "Đơn giá không được là số âm.";
            if (supplierId <= 0 || productId <= 0) return "Thông tin ID không hợp lệ.";

            try
            {
                // Gọi DAL thực thi Stored Procedure
                bool result = _inventoryRepo.ImportGoods(supplierId, userId, productId, qty, price);

                return result ? "Nhập kho thành công!" : "Không thể nhập kho. Vui lòng kiểm tra lại dữ liệu.";
            }
            catch (Exception ex)
            {
                // Ghi log lỗi tại đây nếu cần
                return "Lỗi hệ thống: " + ex.Message;
            }
        }

        // 2. Logic Cảnh báo tồn kho thấp
        public DataTable GetLowStockAlert(int threshold)
        {
            // Nếu threshold không truyền vào, mặc định là 10 (tùy nghiệp vụ)
            if (threshold < 0) threshold = 10;

            return _inventoryRepo.GetLowStockAlert(threshold);
        }

        // 3. Logic Lấy lịch sử sản phẩm
        public DataTable GetProductHistory(int productId)
        {
            if (productId <= 0) return new DataTable(); // Trả về bảng trống nếu ID sai

            return _inventoryRepo.GetStockHistory(productId);
        }
    }
}
