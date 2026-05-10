using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Warehouse.Models;

namespace Warehouse.DAL.Repositories
{
    public class RoleRepository
    {
        private readonly DatabaseHelper _db;
        public RoleRepository(DatabaseHelper db) { _db = db; }

        public List<Role> GetAll()
        {
            var list = new List<Role>();
            using (SqlConnection conn = _db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllRoles", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    list.Add(new Role
                    {
                        RoleId = Convert.ToInt32(dr["role_id"]),
                        RoleName = dr["role_name"].ToString(),
                        Description = dr["description"].ToString()
                    });
                }
            }
            return list;
        }
    }
}