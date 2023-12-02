using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
namespace WebBanQuanAo.Models
{
    public class Product
    {
        // Thuộc tính ProductId đại diện cho ID của Product
        // Áp dụng các Attributes: Required (yêu cầu giá trị) và Key (khóa chính)
        [Required, Key]
        public int ProductId { get; set; }
        // Thuộc tính ProductName lưu trữ tên của Product
        // Yêu cầu giá trị bắt buộc
        [Required]
        public string ProductName { get; set; } = "";
        // Thuộc tính CategoryId lưu trữ ID của Category (loại sản phẩm)
        public int CategoryId { get; set; }
        // Thuộc tính Category đại diện cho Category (loại sản phẩm) của Product
        // Áp dụng ForeignKey để ánh xạ với Category thông qua CategoryId
        [ForeignKey("CategoryId")]
        public  Category? Category { get; set; }
        // Thuộc tính SupplierId lưu trữ ID của Supplier (nhà cung cấp)
        public int SupplierId { get; set; }
        // Thuộc tính Supplier đại diện cho Supplier (nhà cung cấp) của Product
        // Áp dụng ForeignKey để ánh xạ với Supplier thông qua SupplierId
        [ForeignKey("SupplierId")]
        public  Supplier? Supplier { get; set; }
        // Thuộc tính UnitSize lưu trữ kích thước đơn vị của Product
        // Yêu cầu giá trị bắt buộc
        public string? UnitSize { get; set; }
        // Thuộc tính UnitPrice lưu trữ giá đơn vị của Product
        // Yêu cầu giá trị bắt buộc
        public double? UnitPrice { get; set; }
        // Thuộc tính Description lưu trữ mô tả của Product
        // Yêu cầu giá trị bắt buộc
        public string? Description { get; set; }
        // Thuộc tính ImagePath lưu trữ đường dẫn đến hình ảnh của Product
        // Cho phép giá trị null và không yêu cầu bắt buộc
        [AllowNull]
        public string? ImagePath { get; set; }
        // Thuộc tính Active xác định trạng thái hoạt động của Product
        // Yêu cầu giá trị bắt buộc và có thông báo lỗi cụ thể khi trường này rỗng
        [Required(ErrorMessage = "Active not empty!")]
        public bool Active { get; set; }
        // Thuộc tính ImageFile đại diện cho tệp hình ảnh của Product
        // Không ánh xạ với cơ sở dữ liệu (NotMapped) và cho phép giá trị null
        [NotMapped, AllowNull]
        public IFormFile? ImageFile { get; set; }
    }

}

