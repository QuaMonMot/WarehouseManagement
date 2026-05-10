using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Warehouse.Models;

namespace Warehouse.DAL.Repositories
{
    public class ProductRepository
    {
        private readonly DatabaseHelper _db;
        public ProductRepository(DatabaseHelper db) { _db = db; }

        public List<Product> GetAll()
        {
            var list = new List<Product>();
            using (SqlConnection conn = _db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllProducts", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Product
                    {
                        ProductId = Convert.ToInt32(dr["product_id"]),
                        ProductName = dr["product_name"].ToString(),
                        Unit = dr["unit"].ToString(),
                        Price = Convert.ToDecimal(dr["price"]),
                        CategoryId = Convert.ToInt32(dr["category_id"])
                        // Nếu model Product có thêm CategoryName thì gán:
                        // CategoryName = dr["category_name"].ToString()
                    });
                }
            }
            return list;
        }

        public bool Insert(Product p)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", p.ProductName);
                cmd.Parameters.AddWithValue("@CategoryId", p.CategoryId);
                cmd.Parameters.AddWithValue("@Unit", p.Unit ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Price", p.Price);

                conn.Open();
                // Thay vì ExecuteNonQuery, hãy dùng ExecuteScalar để lấy số 1 từ Procedure trả về
                var result = cmd.ExecuteScalar();
                return result != null && Convert.ToInt32(result) == 1;
            }
        }
        public bool Export(int userId, int productId, int qty)
        {
            using (var conn = _db.GetConnection())
            {
                var cmd = new SqlCommand("sp_ExportProduct", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                cmd.Parameters.AddWithValue("@Quantity", qty);

                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) == 1;
            }
        }
    }
}