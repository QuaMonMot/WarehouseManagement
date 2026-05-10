using System.Data;
using System.Data.SqlClient;

namespace Warehouse.DAL
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        // QUAN TRỌNG: Thêm hàm khởi tạo nhận tham số này vào
        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public SqlConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}