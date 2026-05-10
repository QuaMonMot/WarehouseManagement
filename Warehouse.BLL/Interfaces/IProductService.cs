using System.Collections.Generic;
using Warehouse.Models;

namespace Warehouse.BLL.Interfaces
{
    public interface IProductService
    {
        List<Product> GetProducts();
        string AddProduct(Product p);
    }
}