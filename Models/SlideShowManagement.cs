using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WebBanQuanAo.Models
{
    public class SlideShowManagement
    {
        // Thuộc tính SlideShowId đại diện cho ID của SlideShow
        // Áp dụng các Attributes: Required (yêu cầu giá trị) và Key (khóa chính)
        [Required, Key]
        public int SlideShowId { get; set; }
        // Thuộc tính Title lưu trữ tiêu đề của SlideShow
        // Yêu cầu giá trị bắt buộc và mặc định là chuỗi rỗng
        [Required]
        public string Title { get; set; } = "";
        // Thuộc tính ImagePath lưu trữ đường dẫn đến hình ảnh của SlideShow
        // Cho phép giá trị null và không yêu cầu bắt buộc
        [AllowNull]
        public string? ImagePath { get; set; }
        //Thuộc tính Active xác định trạng thái hoạt động của SlideShow
        // Yêu cầu giá trị bắt buộc và có thông báo lỗi cụ thể khi trường này rỗng
        [Required(ErrorMessage = "Active not empty!")]
        public bool Active { get; set; }
        // Thuộc tính ImageFile đại diện cho tệp hình ảnh của SlideShowManagement
        // Không ánh xạ với cơ sở dữ liệu (NotMapped) và cho phép giá trị null
        [NotMapped, AllowNull]
        public IFormFile? ImageFile { get; set; }
    }
}