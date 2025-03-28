using Microsoft.AspNetCore.Mvc;

namespace DemoWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private const string UploadPath = @"D:\UploadFile"; // Thư mục lưu file

        [HttpPost("upload")]
        public async Task<IActionResult> UploadFile([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(new { status = 0, message = "Vui lòng chọn file để upload." });
            }

            try
            {
                // Tạo thư mục nếu chưa có
                if (!Directory.Exists(UploadPath))
                {
                    Directory.CreateDirectory(UploadPath);
                }

                string filePath = Path.Combine(UploadPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok(new { status = 1, message = "Upload file thành công!", fileName = file.FileName });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = 0, message = "Lỗi khi upload file", error = ex.Message });
            }
        }

    [HttpGet("download/{fileName}")]
        public IActionResult DownloadFile(string fileName)
        {
            string filePath = Path.Combine(UploadPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound(new { status = 0, message = "File không tồn tại." });
            }

            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/octet-stream", fileName);
        }
    }
}
