using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Models
{
    public class InventoryReport
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Unit { get; set; }
        public int QtyImport { get; set; }   // Tổng nhập
        public int QtyExport { get; set; }   // Tổng xuất
        public int CurrentInventory { get; set; } // Tồn hiện tại
    }
}
