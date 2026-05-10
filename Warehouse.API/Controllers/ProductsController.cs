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
            if (result.Contains("thành công")) return Ok(result);
            return BadRequest(result);
        }
    }
}