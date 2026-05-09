using System.Data;
using System.Data.SqlClient;
using Warehouse.DAL.DbContext;

namespace Warehouse.DAL.Repositories
{
    public class StockRepository
    {
        private readonly SqlConnectionFactory _db;
        public StockRepository(SqlConnectionFactory db) => _db = db;

        // Hàm cập nhật tồn kho bằng ADO.NET
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