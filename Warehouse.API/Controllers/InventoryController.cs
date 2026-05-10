using Microsoft.AspNetCore.Mvc;
using Warehouse.BLL.Interfaces;

namespace Warehouse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        [HttpPost("import")]
        public IActionResult Import(int supplierId, int userId, int productId, int qty, decimal price)
        {
            var result = _inventoryService.ImportGoods(supplierId, userId, productId, qty, price);
            return Ok(result);
        }

        [HttpGet("low-stock")]
        public IActionResult GetLowStock(int threshold = 10)
        {
            var data = _inventoryService.GetLowStockAlert(threshold);
            return Ok(data); // Lưu ý: DataTable sẽ tự convert sang JSON
        }

        [HttpGet("history/{productId}")]
        public IActionResult GetHistory(int productId)
        {
            return Ok(_inventoryService.GetProductHistory(productId));
        }
    }
}