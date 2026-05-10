using Microsoft.AspNetCore.Mvc;
using Warehouse.BLL.Interfaces;
using Warehouse.Models;
using Warehouse.Models.DTOs;
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

        public IActionResult Add(
         [FromBody] CreateProductDTO dto
 )
        {
            try
            {
                Product product = new Product
                {
                    SKU = dto.SKU,

                    ProductName = dto.ProductName,

                    Quantity = dto.Quantity,

                    Price = dto.Price,

                    MinStock = dto.MinStock,

                    SupplierId = dto.SupplierId
                };

                _productService.Add(product);

                return Ok("Add success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================
        // SỬA SẢN PHẨM
        // =========================
        [HttpPut("{id}")]
        [EndpointSummary("Sửa sản phẩm")]

        public IActionResult Update(
            int id,
            [FromBody] UpdateProductDTO dto
)
        {
            try
            {
                Product product = new Product
                {
                    ProductId = id,

                    SKU = dto.SKU,

                    ProductName = dto.ProductName,

                    Quantity = dto.Quantity,

                    Price = dto.Price,

                    MinStock = dto.MinStock,

                    SupplierId = dto.SupplierId
                };

                _productService.Update(product);

                return Ok("Update success");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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