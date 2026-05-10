using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Warehouse.Models;

namespace Warehouse.DAL.Repositories
{
    public class UserRepository
    {
        private readonly DatabaseHelper _db;
        public UserRepository(DatabaseHelper db) { _db = db; }
        public User Login(string username, string passHash)
        {
            using (SqlConnection conn = _db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_Login", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@PasswordHash", passHash);

                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    return new User
                    {
                        UserId = (int)dr["user_id"],
                        Username = dr["username"].ToString(),
                        FullName = dr["full_name"].ToString(),
                        // Bạn có thể dùng RoleName từ bảng Role nếu Model có thuộc tính này
                    };
                }
                return null;
            }
        }
    }
}