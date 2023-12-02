using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebBanQuanAo.Models
{
    public class Order
    {
        // Thuộc tính OrderId đại diện cho ID của đơn hàng
        // Áp dụng các Attributes: Required (yêu cầu giá trị) và Key (khóa chính)
        [Required, Key]
        public int OrderId { get; set; }
        // Thuộc tính CustomerId lưu trữ ID của khách hàng đặt hàng
        public int CustomerId { get; set; }
        // Thuộc tính CustomerInfo đại diện cho thông tin khách hàng liên quan đến đơn hàng
        // Áp dụng ForeignKey để ánh xạ với CustomerInfo thông qua CustomerId
        [ForeignKey("CustomerId")]
        public  CustomerInfo? CustomerInfo { get; set; }
        // Thuộc tính OrderDate lưu trữ ngày đặt hàng của đơn hàng
        public DateTime OrderDate { get; set; }
        // Thuộc tính ShipperId lưu trữ ID của Shipper (người vận chuyển) liên quan đến đơn hàng
        public int ShipperId { get; set; }
        // Thuộc tính Shipper đại diện cho thông tin Shipper liên quan đến đơn hàng
        // Áp dụng ForeignKey để ánh xạ với Shipper thông qua ShipperId
        [ForeignKey("ShipperId")]
        public  Shipper? Shipper { get; set; }
        // Thuộc tính Active xác định trạng thái hoạt động của đơn hàng
        public bool Active { get; set; }
        // Thuộc tính Payments lưu trữ danh sách các thanh toán liên quan đến đơn hàng
        public ICollection<Payment>? Payments { get; set; }
        // Thuộc tính OrderDetails lưu trữ danh sách các chi tiết đơn hàng liên quan đến đơn hàng
        public ICollection<OrderDetail>? OrderDetails { get; set; }
       
    }
}
