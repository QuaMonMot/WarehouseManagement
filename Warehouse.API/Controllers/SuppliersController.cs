using Microsoft.AspNetCore.Mvc;
using Warehouse.BLL.Interfaces;
using Warehouse.Models;

namespace Warehouse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public IActionResult Get() => Ok(_supplierService.GetAllActive());

        [HttpPost]
        public IActionResult Post(Supplier s) => Ok(_supplierService.AddSupplier(s));

        [HttpDelete("{id}")]
        public IActionResult Delete(int id) => Ok(_supplierService.DeleteSupplier(id));
    }
}