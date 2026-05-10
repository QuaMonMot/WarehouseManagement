using System;
using System.Data;

namespace Warehouse.BLL.Interfaces
{
    public interface IReportService
    {
        // Thống kê Dashboard tổng quát
        DataTable GetDashboardSummary();

        // Báo cáo luồng kho theo khoảng thời gian
        DataTable GetInventoryFlowReport(DateTime? fromDate, DateTime? toDate);
    }
}