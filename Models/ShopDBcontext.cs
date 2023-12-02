using Microsoft.EntityFrameworkCore;
using WebBanQuanAo.Models;
namespace WebBanQuanAo.Models
{
    public class ShopDBcontext : DbContext
    {
        public ShopDBcontext(DbContextOptions<ShopDBcontext> options) : base(options) { }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Supplier> Suppliers{ get; set; }
        public virtual DbSet<CustomerInfo> CustomerInfo { get; set; }
        public virtual DbSet<SlideShowManagement> SlideShowManagement { get; set; }
        public virtual DbSet<Order> Order { get; set; }
        public virtual DbSet<Payment> Payment { get; set; }
        public virtual DbSet<OrderDetail> OrderDetail{ get; set; }
        public virtual DbSet<Shipper> Shipper { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Login> Logins { get; set; }

    }
}
