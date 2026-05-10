using System.Data;
using Microsoft.Data.SqlClient;
using Warehouse.DAL.DbContext;
using Warehouse.DAL.Interfaces;

namespace Warehouse.DAL.Repositories
{
    public class StockRepository : IStockRepository
    {
        private readonly SqlConnectionFactory _db;
        public StockRepository(SqlConnectionFactory db) => _db = db;

        public async Task<bool> UpdateStockAsync(int productId, int quantityChange)
        {
            using var conn = _db.CreateConnection();
            string sql = "UPDATE Products SET CurrentStock = CurrentStock + @Change WHERE Id = @Id";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Change", quantityChange);
            cmd.Parameters.AddWithValue("@Id", productId);

            await conn.OpenAsync();
            return await cmd.ExecuteNonQueryAsync() > 0;
        }
    }
}