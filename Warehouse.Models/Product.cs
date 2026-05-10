using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Models
{
    public class Product : BaseEntity
    {
        [Key]
        public int ProductId { get; set; }

        [Required]
        [StringLength(150)]
        public string ProductName { get; set; }

        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }

        [Required]
        public string Unit { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Thuộc tính điều hướng về Category
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }
    }
}
    