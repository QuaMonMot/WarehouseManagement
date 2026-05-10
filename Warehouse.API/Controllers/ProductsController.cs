using Microsoft.AspNetCore.Mvc;
using Warehouse.BLL.Interfaces;
using Warehouse.Models;

namespace Warehouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        // =========================
        // LẤY DANH SÁCH SẢN PHẨM
        // =========================
        [HttpGet]

        [EndpointSummary("Lấy danh sách sản phẩm")]
        public IActionResult GetAll()
        {
            var products = _productService.GetAll();

            return Ok(products);
        }

        // =========================
        // LẤY SẢN PHẨM THEO ID
        // =========================
        [HttpGet("{id}")]

        [EndpointSummary("Lấy sản phẩm theo ID")]
        public IActionResult GetById(int id)
        {
            var product = _productService.GetById(id);

            return Ok(product);
        }

        // =========================
        // THÊM SẢN PHẨM
        // =========================
        [HttpPost]

        [EndpointSummary("Thêm sản phẩm")]
        public IActionResult Add(Product product)
        {
            _productService.Add(product);

            return Ok("Add success");
        }

        // =========================
        // SỬA SẢN PHẨM
        // =========================
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Product product)
        {
            product.ProductId = id;

            _productService.Update(product);

            return Ok("Update success");
        }

        // =========================
        // XÓA SẢN PHẨM
        // =========================
        [HttpDelete("{id}")]

        [EndpointSummary("Xóa sản phẩm")]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);

            return Ok("Delete success");
        }
    }
}