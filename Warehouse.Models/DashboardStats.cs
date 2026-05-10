using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Models
{
    public class DashboardStats
    {
        public int TotalProducts { get; set; }
        public int TotalSuppliers { get; set; }
        public int TotalStock { get; set; }
        public decimal TotalImportValue { get; set; }
    }
}
