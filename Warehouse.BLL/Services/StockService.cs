using Warehouse.BLL.Interfaces;
using Warehouse.DAL.Interfaces;
using Warehouse.Models;

namespace Warehouse.BLL.Services
{
    public class StockService : IStockService
    {
        private readonly IStockRepository
            _stockRepository;

        public StockService(
            IStockRepository stockRepository
        )
        {
            _stockRepository = stockRepository;
        }

        public void ImportStock(
            int productId,
            int quantity,
            string note
        )
        {
            ValidateStockMovement(productId, quantity);

            _stockRepository.ImportStock(
                productId,
                quantity,
                note ?? string.Empty
            );
        }

        public void ExportStock(
            int productId,
            int quantity,
            string note
        )
        {
            ValidateStockMovement(productId, quantity);

            _stockRepository.ExportStock(
                productId,
                quantity,
                note ?? string.Empty
            );
        }

        public List<Product> GetInventory()
        {
            return _stockRepository.GetInventory();
        }

        public List<Product> GetLowStock()
        {
            return _stockRepository.GetLowStock();
        }

        public List<StockLog> GetHistory()
        {
            return _stockRepository.GetHistory();
        }
        public StockDashboard GetDashboard()
        {
            return _stockRepository.GetDashboard();
        }

        public List<Product> Search(string keyword)
        {
            return _stockRepository.Search(keyword ?? string.Empty);
        }

        public List<Product> Paging( int page,int pageSize)
        {
            if (page <= 0)
            {
                throw new ArgumentException("Page must be greater than 0");
            }

            if (pageSize <= 0)
            {
                throw new ArgumentException("Page size must be greater than 0");
            }

            return _stockRepository.Paging(
                page,
                pageSize
            );
        }

        public object GetReport()
        {
            return _stockRepository.GetReport();
        }

        private static void ValidateStockMovement(int productId, int quantity)
        {
            if (productId <= 0)
            {
                throw new ArgumentException("Product id must be greater than 0");
            }

            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than 0");
            }
        }
    }
}
