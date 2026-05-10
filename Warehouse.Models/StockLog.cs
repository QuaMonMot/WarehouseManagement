namespace Warehouse.Models
{
    public class StockLog
    {
        public int LogId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public string Type { get; set; }

        public string Note { get; set; }
    }
}