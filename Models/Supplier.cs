using System.ComponentModel.DataAnnotations;

namespace WebBanQuanAo.Models
{
    public class Supplier
    {
        // Thuộc tính SupplierId đại diện cho ID của nhà cung cấp
        // Áp dụng các Attributes: Required (yêu cầu giá trị) và Key (khóa chính)
        [Required, Key]
        public int SupplierId { get; set; }
        // Thuộc tính SupplierName lưu trữ tên của nhà cung cấp
        // Yêu cầu giá trị bắt buộc và có thông báo lỗi cụ thể khi trường này rỗng
        [Required(ErrorMessage = "Vui lòng nhập SupplierName")]
        public string SupplierName { get; set; } = "";
        // Thuộc tính Address lưu trữ địa chỉ của nhà cung cấp
        // Yêu cầu giá trị bắt buộc và có thông báo lỗi cụ thể khi trường này rỗng
        [Required(ErrorMessage = "Vui lòng nhập Address")]
        public string Address { get; set; } = "";
        // Thuộc tính Phone lưu trữ số điện thoại của nhà cung cấp
        // Yêu cầu giá trị bắt buộc và có thông báo lỗi cụ thể khi trường này rỗng
        [Required(ErrorMessage = "Vui lòng nhập Phone")]
        public string Phone { get; set; } = "";
        // Thuộc tính Active xác định trạng thái hoạt động của nhà cung cấp
        public bool Active { get; set; }
        // Thuộc tính Products lưu trữ danh sách sản phẩm của nhà cung cấp
        public ICollection<Product>? Products { get; set; }
    }
}
