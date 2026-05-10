using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Models
{
    public class ImportOrder : BaseEntity
    {
        public int ImportId { get; set; }
        public int SupplierId { get; set; }
        public int UserId { get; set; }
        public string Status { get; set; } // "Đã hoàn thành", "Đã hủy"
        public decimal TotalAmount { get; set; }

        public virtual Supplier Supplier { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<ImportOrderDetail> ImportOrderDetails { get; set; }
    }
}
