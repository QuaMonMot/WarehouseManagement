using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Models
{
    public class StockHistory
    {
        public DateTime ActionDate { get; set; }
        public string ActionType { get; set; } // "Nhập kho" hoặc "Xuất kho"
        public int Amount { get; set; }        // Số lượng (âm hoặc dương)
        public string PerformedBy { get; set; } // Tên nhân viên thực hiện
    }
}
