using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Models
{
    public class ImportOrderDetail
    {
        public int ImportId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        public virtual ImportOrder ImportOrder { get; set; }
        public virtual Product Product { get; set; }
    }
}
