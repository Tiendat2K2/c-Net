using System.ComponentModel.DataAnnotations;

namespace WebBanQuanAo.Models
{
    public class Login
    {
        // Thuộc tính Id đại diện cho ID của thông tin đăng nhập
        // Áp dụng các Attributes: Required (yêu cầu giá trị) và Key (khóa chính)
        [Required, Key]
        public int Id { get; set; }
        // Thuộc tính Username lưu trữ tên người dùng khi đăng nhập
        // Yêu cầu giá trị bắt buộc và có thông báo lỗi cụ thể khi trường này rỗng
        [Required]
        public string Username { get; set; } = "";
        // Thuộc tính Password lưu trữ mật khẩu khi đăng nhập
        // Yêu cầu giá trị bắt buộc
        [Required]
        public string Password { get; set; } = "";



    }
}
