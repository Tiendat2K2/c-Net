using System.ComponentModel.DataAnnotations;

namespace WebBanQuanAo.Models
{
    public class CustomerInfo
    {
        [Required, Key]
        // Thuộc tính yêu cầu và xác định trường CustomerId là một trường khóa chính
        public int CustomerId { get; set; }
        // ID của khách hàng
        
        [Required(ErrorMessage = ("Vui lòng nhập CustomerName"))]
        // Thuộc tính yêu cầu CustomerName không được để trống, với thông báo lỗi cụ thể khi không được cung cấp
        public string CustomerName { get; set; } = "";
        // Tên của khách hàng, giá trị mặc định là chuỗi rỗng

        [Required(ErrorMessage = ("Vui lòng nhập Address"))]
        // Thuộc tính yêu cầu Address không được để trống, với thông báo lỗi cụ thể khi không được cung cấp
        public string Address { get; set; } = "";
        // Địa chỉ của khách hàng, giá trị mặc định là chuỗi rỗng

        [Required(ErrorMessage = ("Vui lòng nhập Phone"))]
        // Thuộc tính yêu cầu Phone không được để trống, với thông báo lỗi cụ thể khi không được cung cấp
        public string Phone { get; set; } = "";
        // Số điện thoại của khách hàng, giá trị mặc định là chuỗi rỗng
        public bool Active { get; set; }
        // Thuộc tính Active xác định trạng thái hoạt động của thông tin khách hàng
        public ICollection<Order>? Orders { get; set; }
        // Một ICollection chứa danh sách các đơn đặt hàng của khách hàng này
        // Dấu hỏi cho biết trường này có thể nhận giá trị null
    }
}
