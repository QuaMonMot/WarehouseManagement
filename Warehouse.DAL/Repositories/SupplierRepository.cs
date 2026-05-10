using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Warehouse.Models;

namespace Warehouse.DAL.Repositories
{
    public class SupplierRepository
    {
        private readonly DatabaseHelper _db;
        public SupplierRepository(DatabaseHelper db) { _db = db; }

        // Gọi SP Lấy danh sách
        public List<Supplier> GetAll()
        {
            var list = new List<Supplier>();
            using (SqlConnection conn = _db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllSuppliers", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Supplier
                    {
                        SupplierId = Convert.ToInt32(dr["supplier_id"]),
                        SupplierName = dr["supplier_name"].ToString(),
                        Phone = dr["phone"].ToString(),
                        Address = dr["address"].ToString(),
                        Email = dr["email"].ToString()
                    });
                }
            }
            return list;
        }

        // Gọi SP Thêm mới
        public bool Insert(Supplier s)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertSupplier", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", s.SupplierName);
                cmd.Parameters.AddWithValue("@Phone", (object)s.Phone ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Address", (object)s.Address ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)s.Email ?? DBNull.Value);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Gọi SP Xóa mềm
        public bool SoftDelete(int id)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_SoftDeleteSupplier", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}