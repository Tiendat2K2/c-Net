using System.ComponentModel.DataAnnotations;

namespace WebBanQuanAo.Models
{
    public class Shipper
    {
        // Thuộc tính ShipperId đại diện cho ID của Shipper
        // Áp dụng các Attributes: Required (yêu cầu giá trị) và Key (khóa chính)
        [Required, Key]
        public int ShipperId { get; set; }
        // Thuộc tính ShipperName lưu trữ tên của Shipper
        // Áp dụng Attribute Required và có thông báo lỗi cụ thể khi không có giá tr
        [Required(ErrorMessage = "Vui lòng nhập ShipperName")]
        public string ShipperName { get; set; } = "";
        // Thuộc tính Phone lưu trữ số điện thoại của Shipper
        // Áp dụng Attribute Required và có thông báo lỗi cụ thể khi không có giá trị
        [Required(ErrorMessage = "Vui lòng nhập SĐT")]
        public string Phone { get; set; } = "";
        // Thuộc tính Active xác định trạng thái hoạt động của Shipper
        public bool Active { get; set; }
        // Thuộc tính Orders lưu trữ danh sách các đơn hàng (ICollection<Order>) của Shipper
        // Có thể là null (dấu hỏi chấm) và có thể chứa nhiều đơn hàng
        public ICollection<Order>? Orders { get; set; }
    }
}
