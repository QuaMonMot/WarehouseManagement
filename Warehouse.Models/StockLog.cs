namespace Warehouse.Models
{
    public class StockLog
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Type { get; set; } 
        public int Quantity { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public string Note { get; set; }
    }
}
