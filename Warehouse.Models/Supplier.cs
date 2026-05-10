using System.ComponentModel.DataAnnotations;

namespace Warehouse.Models
{
    public class Supplier : BaseEntity
    {
        public Supplier()
        {
            ImportOrders = new HashSet<ImportOrder>();
        }

        [Key]
        public int SupplierId { get; set; }

        [Required(ErrorMessage = "Tên nhà cung cấp không được để trống")]
        [StringLength(150)]
        public string SupplierName { get; set; }

        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100)]
        public string Email { get; set; }

        // Quan hệ: Một nhà cung cấp có thể có nhiều phiếu nhập hàng
        // Sử dụng virtual để hỗ trợ Lazy Loading nếu cần
        public virtual ICollection<ImportOrder> ImportOrders { get; set; }
        
    }

}
