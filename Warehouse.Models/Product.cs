namespace Warehouse.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string SKU { get; set; } 
        public string Name { get; set; }
        public int CurrentStock { get; set; } 
        public int MinStockLevel { get; set; } 
        public decimal Price { get; set; }
    }
}
