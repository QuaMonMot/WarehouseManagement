using Microsoft.AspNetCore.Mvc;
using Warehouse.BLL.Interfaces;
using Warehouse.Models;
using Warehouse.Models.DTOs;

namespace Warehouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(
            ISupplierService supplierService
        )
        {
            _supplierService = supplierService;
        }

        // =========================
        // GET ALL
        // =========================
        [HttpGet]
        [EndpointSummary("Lấy danh sách nhà cung cấp")]

        public IActionResult GetAll()
        {
            return Ok(
                _supplierService.GetAll()
            );
        }

        // =========================
        // GET BY ID
        // =========================
        [HttpGet("{id}")]
        [EndpointSummary("Lấy thông tin nhà cung cấp theo ID")]

        public IActionResult GetById(int id)
        {
            return Ok(
                _supplierService.GetById(id)
            );
        }

        // =========================
        // ADD
        // =========================
        [HttpPost]
        [EndpointSummary("Thêm nhà cung cấp mới")]

        public IActionResult Add(
            [FromBody] CreateSupplierDTO dto
        )
        {
            Supplier supplier = new Supplier
            {
                SupplierCode = dto.SupplierCode,

                SupplierName = dto.SupplierName,

                Phone = dto.Phone,

                Address = dto.Address
            };

            _supplierService.Add(supplier);

            return Ok("Add success");
        }

        // =========================
        // UPDATE
        // =========================
        [HttpPut("{id}")]
        [EndpointSummary("Sửa thông tin nhà cung cấp")]

        public IActionResult Update(
            int id,
            [FromBody] UpdateSupplierDTO dto
        )
        {
            Supplier supplier = new Supplier
            {
                SupplierId = id,

                SupplierCode = dto.SupplierCode,

                SupplierName = dto.SupplierName,

                Phone = dto.Phone,

                Address = dto.Address
            };

            _supplierService.Update(supplier);

            return Ok("Update success");
        }

        // =========================
        // DELETE
        // =========================
        [HttpDelete("{id}")]
        [EndpointSummary("Xoá nhà cung cấp")]

        public IActionResult Delete(int id)
        {
            _supplierService.Delete(id);

            return Ok("Delete success");
        }
    }
}