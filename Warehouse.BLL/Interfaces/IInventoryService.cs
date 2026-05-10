using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.BLL.Interfaces
{
    public interface IInventoryService
    {
        // Nhập kho
        string ImportGoods(int supplierId, int userId, int productId, int qty, decimal price);

        // Cảnh báo tồn kho thấp (Trả về DataTable hoặc List tùy bạn)
        DataTable GetLowStockAlert(int threshold);

        // Lịch sử biến động
        DataTable GetProductHistory(int productId);
    }
}
