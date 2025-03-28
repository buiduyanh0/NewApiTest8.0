using DemoWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace DemoWebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeeController : ControllerBase
{
    private static readonly List<string> employeeNames = new List<string>
        {
            "Nguyễn Văn A", "Trần Thị B", "Lê Minh C",
            "Phạm Thanh D", "Vũ Hoàng E", "Đặng Lan F",
            "Hoàng Anh G", "Bùi Quang H", "Cao Kim I", "Lý Xuân J"
        };

    private readonly ILogger<EmployeeController> _logger;

    private const string FilePath = "employee.txt";

    public EmployeeController(ILogger<EmployeeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetEmployees()
    {
        return Ok(new
        {
            status = 1,
            data = employeeNames
        });
    }

    [HttpGet("search")]
    public IActionResult SearchEmployees([FromQuery] string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            return BadRequest(new { status = 0, message = "Vui lòng nhập từ khóa tìm kiếm." });
        }

        var result = employeeNames
            .Where(e => e.Contains(name, StringComparison.OrdinalIgnoreCase))
            .ToList();

        return Ok(new
        {
            status = 1,
            data = result
        });
    }

    [HttpPost("postemployee")]
    public IActionResult SaveEmployee([FromBody] Employee employee)
    {
        if (employee == null || string.IsNullOrWhiteSpace(employee.EmployeeId) || string.IsNullOrWhiteSpace(employee.EmployeeName))
        {
            return BadRequest(new { status = 0, message = "Dữ liệu không hợp lệ." });
        }

        try
        {
            string employeeData = $"Mã nhân viên: {employee.EmployeeId}\nTên nhân viên: {employee.EmployeeName}\n\n";

            System.IO.File.AppendAllText(FilePath, employeeData, Encoding.UTF8);

            return Ok(new { status = 1, message = "Lưu nhân viên thành công." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { status = 0, message = "Lỗi hệ thống", error = ex.Message });
        }
    }
}
