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
            try
            {
                var supplier = _supplierService.GetById(id);

                if (supplier == null)
                {
                    return NotFound("Supplier not found");
                }

                return Ok(supplier);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
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
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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
            try
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
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================
        // DELETE
        // =========================
        [HttpDelete("{id}")]
        [EndpointSummary("Xoá nhà cung cấp")]

        public IActionResult Delete(int id)
        {
            try
            {
                _supplierService.Delete(id);

                return Ok("Delete success");
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
