using System.Collections.Generic;
using Warehouse.Models;

namespace Warehouse.BLL.Interfaces
{
    public interface ISupplierService
    {
        List<Supplier> GetAllActive();
        string AddSupplier(Supplier s);
        string UpdateSupplier(Supplier s); // Thêm hàm sửa nếu bạn đã viết SP sửa
        bool DeleteSupplier(int id);
    }
}