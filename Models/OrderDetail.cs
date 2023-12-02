using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBanQuanAo.Models
{
    public class OrderDetail
    {
        // Thuộc tính OrderDetailId đại diện cho ID của chi tiết đơn hàng
        // Áp dụng các Attributes: Required (yêu cầu giá trị) và Key (khóa chính)
        [Required, Key]
        public int OrderDetailId { get; set; }
        // Thuộc tính OrderId lưu trữ ID của đơn hàng liên quan đến chi tiết đơn hàng
        public int OrderId { get; set; }
        // Thuộc tính Order đại diện cho đơn hàng liên quan đến chi tiết đơn hàng
        // Áp dụng ForeignKey để ánh xạ với Order thông qua OrderId
        [ForeignKey("OrderId")]
        public  Order? Order { get; set; }
        // Thuộc tính ProductId lưu trữ ID của sản phẩm liên quan đến chi tiết đơn hàng
        public int ProductId { get; set; }
        // Thuộc tính Product đại diện cho sản phẩm liên quan đến chi tiết đơn hàng
        // Áp dụng ForeignKey để ánh xạ với Product thông qua ProductId
        [ForeignKey("ProductId")]
        public  Product? Product { get; set; }
        // Thuộc tính Quantity lưu trữ số lượng sản phẩm trong chi tiết đơn hàng
        public int Quantity { get; set; }
        // Thuộc tính Price lưu trữ giá của sản phẩm trong chi tiết đơn hàng
        // Sử dụng ColumnAttribute để chỉ định loại dữ liệu và định dạng của cột trong cơ sở dữ liệu
        [Column(TypeName = "decimal(10, 2)")]
        public decimal Price { get; set; }
        // Thuộc tính Active xác định trạng thái hoạt động của chi tiết đơn hàng
        public bool Active { get; set; }
    }
}
