using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebBanQuanAo.Models
{
    public class Payment
    {
        // Thuộc tính PaymentId đại diện cho ID của Payment
        // Áp dụng các Attributes: Required (yêu cầu giá trị) và Key (khóa chính)
        [Required, Key]
        public int PaymentId { get; set; }
        // Thuộc tính OrderId lưu trữ ID của Order liên quan đến Payment
        public int OrderId { get; set; }
        // Thuộc tính Order đại diện cho Order liên quan đến Payment
        // Áp dụng ForeignKey để ánh xạ với Order thông qua OrderId
        [ForeignKey("OrderId")]
        public  Order? Order { get; set; }
        // Thuộc tính PaymentDate lưu trữ ngày thanh toán của Payment
        public DateTime PaymentDate { get; set; }
        // Thuộc tính AmountPaid lưu trữ số tiền thanh toán
        // Sử dụng ColumnAttribute để chỉ định loại dữ liệu và định dạng của cột trong cơ sở dữ liệu
        [Column(TypeName = "decimal(10, 2)")]
        public decimal AmountPaid { get; set; }
        // Thuộc tính PaymentMethod lưu trữ phương thức thanh toán
        public string PaymentMethod { get; set; } = "";
        // Thuộc tính Active xác định trạng thái hoạt động của Payment
        public bool Active { get; set; }
    }
}
