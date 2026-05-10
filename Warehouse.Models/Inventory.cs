using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Models
{
    public class Inventory
    {
        [Key]
        public int ProductId { get; set; }
        public int QuantityInStock { get; set; }
        public DateTime LastUpdated { get; set; }
        public virtual Product Product { get; set; }
    }
}
