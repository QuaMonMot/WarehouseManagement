namespace Warehouse.Models.DTOs
{
    public class UpdateProductDTO
    {
        public string SKU { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int MinStock { get; set; }

        public int SupplierId { get; set; }
    }
}