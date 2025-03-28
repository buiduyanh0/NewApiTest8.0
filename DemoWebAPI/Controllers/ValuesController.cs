using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("GetName")]
        public IActionResult GetName()
        {
            return Ok(new {name= "Hello word!" });
        }


        [HttpGet("GetName1")]
        public IActionResult GetNamed1(string name)
        {
            return Ok(new { name = name });
        }


        List<Employee> employees = new List<Employee>();

        [HttpPost("PostName")]
        public IActionResult PostName([FromBody] Employee employee)
        {
            try
            {
                employees.Add(employee);
                return Ok(new { status = 1, data = employees });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = 0, message = ex.Message });
            }
        }


        [HttpPost("uploadfile")]
        public async Task<IActionResult> UploadFile(string path)
        {
            try
            {
                var file = Request.Form.Files;
                //string path = @"\\192.168.1.2\ftp\Upload\Course\";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (file.Count > 0)
                {
                    foreach (var item in file)
                    {
                        // Thực hiện xử lý tệp tin ở đây, ví dụ lưu vào ổ đĩa
                        string filePath = Path.Combine(path, item.FileName);
                        using (var stream = new FileStream(filePath, FileMode.CreateNew))
                        {
                            await item.CopyToAsync(stream);
                        }
                    }
                    return Ok(new
                    {
                        status = 1,
                        message = "File uploaded successfully.",
                        fileName = file
                    });
                }
                else
                {
                    return BadRequest(new
                    {
                        status = 0,
                        message = "No file uploaded."
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    status = 0,
                    message = ex.Message
                });
            }
        }


        public class Employee
        {
            public string Code { get; set; }
            public string FullName { get; set; }
        }
    }
}
