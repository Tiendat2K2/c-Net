using System.ComponentModel.DataAnnotations;

namespace WebBanQuanAo.Models
{
    public class Category
    {
        [Required, Key]
        // Một attribute yêu cầu và xác định trường CategoryId là khóa chính
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui Lòng nhập CategoryName")]
        // Một trường yêu cầu CategoryName không được để trống, với thông báo lỗi cụ thể khi không được cung cấp
        public string CategoryName { get; set; } = "";
        // Thuộc tính Active xác định trạng thái hoạt động của danh mục
        public bool Active { get; set; }
        // Một ICollection chứa danh sách các sản phẩm thuộc danh mục này
        public ICollection<Product>? Products { get; set; }
        // Dấu hỏi cho biết trường này có thể nhận giá trị null
    }
}
