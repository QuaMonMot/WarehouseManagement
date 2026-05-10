using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Warehouse.DAL.Repositories
{
    public class InventoryRepository
    {
        private readonly DatabaseHelper _db;
        public InventoryRepository(DatabaseHelper db) { _db = db; }

        // Gọi SP Nhập hàng
        public bool ImportGoods(int supplierId, int userId, int productId, int qty, decimal price)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ImportGoods", conn);
                cmd.CommandType = CommandType.StoredProcedure; // Rất quan trọng

                // Thêm các tham số
                cmd.Parameters.AddWithValue("@SupplierId", supplierId);
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@Quantity", qty);
                cmd.Parameters.AddWithValue("@UnitPrice", price);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
        public bool Export(int userId, int productId, int quantity)
        {
            using (var conn = _db.GetConnection())
            {
                var cmd = new SqlCommand("sp_ExportProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@Qty", quantity); // Đã sửa từ bước trước
                conn.Open();
                var result = cmd.ExecuteScalar();
                return result != null && Convert.ToInt32(result) == 1;
            }
        }
        
        // Gọi SP Cảnh báo tồn kho
        public DataTable GetLowStockAlert(int threshold)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = _db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetLowStockAlert", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Threshold", threshold);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
        // Lấy lịch sử biến động của 1 sản phẩm
        public DataTable GetStockHistory(int productId)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = _db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetStockHistory", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductId", productId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}