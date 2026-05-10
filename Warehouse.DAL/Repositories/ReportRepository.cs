using System;
using System.Data;
using System.Data.SqlClient;

namespace Warehouse.DAL.Repositories
{
    public class ReportRepository
    {
        private readonly DatabaseHelper _db;
        public ReportRepository(DatabaseHelper db) { _db = db; }

        // Lấy thống kê Dashboard
        public DataTable GetGeneralStats()
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = _db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetGeneralStatistics", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        // Lấy báo cáo luồng kho (Nhập - Xuất - Tồn)
        public DataTable GetInventoryFlow(DateTime fromDate, DateTime toDate)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conn = _db.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ReportInventoryFlow", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FromDate", fromDate);
                cmd.Parameters.AddWithValue("@ToDate", toDate);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }
    }
}