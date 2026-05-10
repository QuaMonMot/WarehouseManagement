using System;
using System.Data;
using Warehouse.BLL.Interfaces;
using Warehouse.DAL.Repositories;

namespace Warehouse.BLL.Services
{
    public class ReportService : IReportService
    {
        private readonly ReportRepository _reportRepo;

        // Sửa lỗi ở đây: Nhận Repo từ DI container thay vì tự 'new'
        public ReportService(ReportRepository reportRepo)
        {
            _reportRepo = reportRepo;
        }

        // 1. Thống kê Dashboard
        public DataTable GetDashboardSummary()
        {
            try
            {
                // Gọi thẳng từ DAL vì SP này không cần tham số đầu vào
                return _reportRepo.GetGeneralStats();
            }
            catch (Exception)
            {
                // Nếu lỗi trả về table rỗng để tránh crash ứng dụng
                return new DataTable();
            }
        }

        // 2. Báo cáo Nhập - Xuất - Tồn
        public DataTable GetInventoryFlowReport(DateTime? fromDate, DateTime? toDate)
        {
            // Logic xử lý ngày mặc định nếu người dùng để trống
            // Ví dụ: Mặc định lấy báo cáo trong 30 ngày gần nhất
            DateTime actualFromDate = fromDate ?? DateTime.Now.AddDays(-30);
            DateTime actualToDate = toDate ?? DateTime.Now;

            // Đảm bảo logic: Ngày bắt đầu không được lớn hơn ngày kết thúc
            if (actualFromDate > actualToDate)
            {
                actualFromDate = actualToDate.AddDays(-30);
            }

            try
            {
                return _reportRepo.GetInventoryFlow(actualFromDate, actualToDate);
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }
    }
}