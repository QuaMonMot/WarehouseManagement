namespace Warehouse.Models.DTOs
{
    public class ExportStockDTO
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string Note { get; set; }
    }
}