using Microsoft.Data.SqlClient;
using Warehouse.DAL.DbContext;
using Warehouse.DAL.Interfaces;
using Warehouse.Models;

namespace Warehouse.DAL.Repositories
{
    public class AuthRepository : IAuthRepository
    {
        private readonly SqlConnectionFactory
            _factory;

        public AuthRepository(
            SqlConnectionFactory factory
        )
        {
            _factory = factory;
        }

        public User Login(
            string username,
            string password
        )
        {
            User user = null;

            using (SqlConnection conn =
                _factory.CreateConnection())
            {
                SqlCommand cmd = new SqlCommand(
                    @"SELECT *
                      FROM Users
                      WHERE Username = @Username
                      AND Password = @Password",
                    conn
                );

                cmd.Parameters.AddWithValue(
                    "@Username",
                    username
                );

                cmd.Parameters.AddWithValue(
                    "@Password",
                    password
                );

                conn.Open();

                SqlDataReader reader =
                    cmd.ExecuteReader();

                if (reader.Read())
                {
                    user = new User
                    {
                        UserId =
                            Convert.ToInt32(
                                reader["UserId"]
                            ),

                        Username =
                            reader["Username"]
                            .ToString(),

                        Role =
                            reader["Role"]
                            .ToString()
                    };
                }
            }

            return user;
        }
    }
}