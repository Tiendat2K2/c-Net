using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanQuanAo.Models;

namespace WebBanQuanAo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly ShopDBcontext _context;

        public OrdersController(ShopDBcontext context)
        {
            _context = context;
        }

        // GET: Admin/Orders
        public async Task<IActionResult> Index()
        {
            var shopDBcontext = _context.Order.Include(o => o.CustomerInfo).Include(o => o.Shipper);
            return View(await shopDBcontext.ToListAsync());
        }

        // GET: Admin/Orders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.CustomerInfo)
                .Include(o => o.Shipper)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // GET: Admin/Orders/Create
        public IActionResult Create()
        {
            // Truyền danh sách CustomerId từ bảng CustomerInfo vào view để người dùng có thể chọn từ dropdown list khi tạo mới Order
            ViewData["CustomerId"] = new SelectList(_context.CustomerInfo, "CustomerId", "Address");
            // Truyền danh sách ShipperId từ bảng Shipper vào view để người dùng có thể chọn từ dropdown list khi tạo mới Order
            ViewData["ShipperId"] = new SelectList(_context.Shipper, "ShipperId", "Phone");
            // Trả về view Create để người dùng có thể tạo mới Order
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderId,CustomerId,OrderDate,ShipperId,Active")] Order order)
        {
            // Kiểm tra tính hợp lệ của dữ liệu nhập vào từ form
            if (ModelState.IsValid)
            {// Thêm order mới vào cơ sở dữ liệu và lưu thay đổi
                _context.Add(order);
                await _context.SaveChangesAsync();
                // Chuyển hướng người dùng về trang danh sách Order (Index)
                return RedirectToAction(nameof(Index));
            }
            // Truyền danh sách CustomerId từ bảng CustomerInfo vào view để người dùng có thể chọn từ dropdown list khi tạo mới Order
            ViewData["CustomerId"] = new SelectList(_context.CustomerInfo, "CustomerId", "Address", order.CustomerId);
            // Truyền danh sách ShipperId từ bảng Shipper vào view để người dùng có thể chọn từ dropdown list khi tạo mới Order
            ViewData["ShipperId"] = new SelectList(_context.Shipper, "ShipperId", "Phone", order.ShipperId);
            // Trả về view Create với dữ liệu của order để người dùng có thể sửa lỗi nhập liệu
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Kiểm tra nếu id không tồn tại hoặc bảng Order trong context là null
            if (id == null || _context.Order == null)
            {
                // Trả về NotFound nếu id không hợp lệ hoặc bảng Order không tồn tại
                return NotFound();
            }
            // Tìm order cụ thể với id tương ứng
            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                // Trả về NotFound nếu order không tồn tại trong cơ sở dữ liệu
                return NotFound();
            }
            // Truyền danh sách CustomerId từ bảng CustomerInfo vào view để người dùng có thể chọn từ dropdown list khi chỉnh sửa Order
            ViewData["CustomerId"] = new SelectList(_context.CustomerInfo, "CustomerId", "Address", order.CustomerId);
            // Truyền danh sách ShipperId từ bảng Shipper vào view để người dùng có thể chọn từ dropdown list khi chỉnh sửa Order
            ViewData["ShipperId"] = new SelectList(_context.Shipper, "ShipperId", "Phone", order.ShipperId);
            // Trả về view Edit với dữ liệu của order để người dùng có thể chỉnh sửa
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,CustomerId,OrderDate,ShipperId,Active")] Order order)
        {
            // Kiểm tra nếu id của order không khớp với id được truyền vào từ route
            if (id != order.OrderId)
            {
                // Trả về NotFound nếu id không khớp
                return NotFound();
            }
            // Kiểm tra tính hợp lệ của dữ liệu nhập vào từ form
            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin của order và lưu thay đổi vào cơ sở dữ liệu
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Kiểm tra nếu xảy ra lỗi concurrency khi cập nhật order
                    if (!OrderExists(order.OrderId))
                    {
                        // Trả về NotFound nếu order không tồn tại trong cơ sở dữ liệu
                        return NotFound();
                    }
                    else
                    {
                        // Nếu có lỗi khác, ném ngoại lệ
                        throw;
                    }
                }
                // Chuyển hướng người dùng về trang danh sách Order (Index) sau khi chỉnh sửa thành công
                return RedirectToAction(nameof(Index));
            }
            // Truyền danh sách CustomerId từ bảng CustomerInfo vào view để người dùng có thể chọn từ dropdown list khi chỉnh sửa Order
            ViewData["CustomerId"] = new SelectList(_context.CustomerInfo, "CustomerId", "Address", order.CustomerId);
            // Truyền danh sách ShipperId từ bảng Shipper vào view để người dùng có thể chọn từ dropdown list khi chỉnh sửa Order
            ViewData["ShipperId"] = new SelectList(_context.Shipper, "ShipperId", "Phone", order.ShipperId);
            // Trả về view Edit với dữ liệu của order để người dùng có thể chỉnh sửa
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order
                .Include(o => o.CustomerInfo)
                .Include(o => o.Shipper)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kiểm tra xem bảng Order trong context có tồn tại không
            if (_context.Order == null)
            {
                // Trả về Problem nếu bảng Order không tồn tại trong context
                return Problem("Entity set 'ShopDBcontext.Order'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                // Xóa order khỏi cơ sở dữ liệu
                _context.Order.Remove(order);
            }
            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
            // Chuyển hướng người dùng về trang danh sách Order (Index) sau khi xóa thành công
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
          return (_context.Order?.Any(e => e.OrderId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Search(string searchString)
        {// Tạo truy vấn ban đầu cho bảng Order với các thông tin CustomerInfo và Shipper được kèm theo
            IQueryable<Order> ordersQuery = _context.Order.Include(o => o.CustomerInfo).Include(o => o.Shipper);
            // Kiểm tra xem chuỗi tìm kiếm có rỗng hoặc null không
            if (!String.IsNullOrEmpty(searchString))
            {// Lọc các đơn hàng theo các tiêu chí tìm kiếm như địa chỉ khách hàng, ngày đặt hàng, số điện thoại Shipper
                ordersQuery = ordersQuery.Where(o =>
                    EF.Functions.Like(o.CustomerInfo.Address, $"%{searchString}%") ||
                    EF.Functions.Like(o.OrderDate.ToString(), $"%{searchString}%") ||
                    EF.Functions.Like(o.Shipper.Phone, $"%{searchString}%")
                );
            }
            // Thực hiện truy vấn và lấy kết quả tìm kiếm
            var searchResults = await ordersQuery.ToListAsync();

            // Truyền kết quả tìm kiếm vào View Index để hiển thị các đơn hàng đã lọc
            return View("Index", searchResults);
        }
    }
}
