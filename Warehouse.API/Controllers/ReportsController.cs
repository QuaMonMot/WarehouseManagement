using Microsoft.AspNetCore.Mvc;
using Warehouse.BLL.Interfaces;

namespace Warehouse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("dashboard")]
        public IActionResult GetDashboard() => Ok(_reportService.GetDashboardSummary());

        [HttpGet("inventory-flow")]
        public IActionResult GetFlow(DateTime? from, DateTime? to)
            => Ok(_reportService.GetInventoryFlowReport(from, to));
    }
}