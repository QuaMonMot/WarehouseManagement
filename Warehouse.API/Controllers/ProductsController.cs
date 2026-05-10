using Microsoft.AspNetCore.Mvc;
using Warehouse.BLL.Interfaces;
using Warehouse.Models;

namespace Warehouse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_productService.GetProducts());
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            var result = _productService.AddProduct(product);

            // Check không phân biệt hoa thường hoặc check nội dung khớp
            if (result == "Thành công")
            {
                return Ok(result); // Swagger sẽ hiện màu xanh 200 OK
            }

            return BadRequest(result); // Chỉ hiện 400 khi thực sự có lỗi
        }
    }
}