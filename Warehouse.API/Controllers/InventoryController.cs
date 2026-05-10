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
        [HttpPost("export")]
        public IActionResult ExportGoods(int userId, int productId, int quantity)
        {
            try
            {
                // Giả sử bạn đã viết Interface IInventoryService và hàm ExportGoods tương ứng
                var result = _inventoryService.ExportGoods(userId, productId, quantity);
                if (result.Contains("thành công")) return Ok(result);
                return BadRequest(result);
            }
            catch (System.Exception ex)
            {
                // Trả về lỗi chi tiết nếu kho không đủ hàng
                return BadRequest(ex.Message);
            }
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