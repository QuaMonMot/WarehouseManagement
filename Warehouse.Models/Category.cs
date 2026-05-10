using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models
{
    public class Category : BaseEntity
    {
        [Key]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string CategoryName { get; set; }

        public string Description { get; set; }

        // Quan hệ 1 - n: Một danh mục có nhiều sản phẩm
        public virtual ICollection<Product> Products { get; set; }
    }
}
