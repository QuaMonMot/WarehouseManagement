namespace Warehouse.Models.DTOs
{
    public class ImportStockDTO
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string Note { get; set; }
    }
}