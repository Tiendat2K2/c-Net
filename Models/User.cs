using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebBanQuanAo.Models
{
    public class User
    {// Thuộc tính ID, là một trường số nguyên, yêu cầu không được null và là khóa chính
        [Required, Key]
        public int ID { get; set; }
        // Thuộc tính UserName, là một chuỗi, yêu cầu không được rỗng và cần phải được nhập
        [Required(ErrorMessage = "Vui Lòng Nhập UserName")]
        public string UserName { get; set; } = "";
        // Thuộc tính Password, là một chuỗi, yêu cầu không được rỗng và cần phải được nhập
        [Required(ErrorMessage = "Vui Lòng Nhập Password")]
        public string Password { get; set; } = "";
        // Thuộc tính Email, là một chuỗi, yêu cầu không được rỗng và cần phải được nhập
        [Required(ErrorMessage = "Vui Lòng Nhập Email")]
        public string Email { get; set; } = "";
        // Thuộc tính role, là một số nguyên có thể có giá trị null
        public int? role { get; set; }
        // Thuộc tính RoleName, không ánh xạ vào cơ sở dữ liệu
        [NotMapped]
        public string? RoleName { get; set; }
        // Thuộc tính Active, là một biến kiểu boolean để biểu thị trạng thái hoạt động hoặc không hoạt động
        public bool Active { get; set; }
    }
}
