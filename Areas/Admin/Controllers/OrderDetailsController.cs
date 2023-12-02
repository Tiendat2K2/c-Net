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
    public class OrderDetailsController : Controller
    {
        private readonly ShopDBcontext _context;

        public OrderDetailsController(ShopDBcontext context)
        {
            _context = context;
        }

        // GET: Admin/OrderDetails
        public async Task<IActionResult> Index()
        {
            var shopDBcontext = _context.OrderDetail.Include(o => o.Order).Include(o => o.Product);
            return View(await shopDBcontext.ToListAsync());
        }

        // GET: Admin/OrderDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderDetail == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderDetailId == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: Admin/OrderDetails/Create
        public IActionResult Create()
        {
            // Nếu dữ liệu không hợp lệ, truyền dữ liệu cần thiết cho View để hiển thị form tạo mới
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId");
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName");
            // Trả về View tạo mới để người dùng có thể sửa lỗi nhập liệu
            return View();
        }

        // POST: Admin/OrderDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("OrderDetailId,OrderId,ProductId,Quantity,Price,Active")] OrderDetail orderDetail)
        {
            // Nếu dữ liệu hợp lệ, thêm chi tiết đơn hàng vào cơ sở dữ liệu và lưu thay đổi
            if (ModelState.IsValid)
            {
                // Nếu dữ liệu hợp lệ, thêm chi tiết đơn hàng vào cơ sở dữ liệu và lưu thay đổi
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                // Chuyển hướng người dùng đến trang Index sau khi tạo thành công
                return RedirectToAction(nameof(Index));
            }
            // Nếu dữ liệu không hợp lệ, truyền dữ liệu cần thiết cho View để hiển thị form tạo mới
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", orderDetail.ProductId);
            // Trả về View tạo mới để người dùng có thể sửa lỗi nhập liệu
            return View(orderDetail);
        }

        // GET: Admin/OrderDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Kiểm tra xem id có tồn tại và danh sách chi tiết đơn hàng có dữ liệu không
            if (id == null || _context.OrderDetail == null)
            {
                return NotFound();
            }
            // Tìm chi tiết đơn hàng cần chỉnh sửa dựa trên id
            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            // Truyền dữ liệu cần thiết cho View để hiển thị thông tin cần chỉnh sửa
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", orderDetail.ProductId);
            // Trả về View hiển thị form chỉnh sửa thông tin chi tiết đơn hàng
            return View(orderDetail);
        }

        // POST: Admin/OrderDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("OrderDetailId,OrderId,ProductId,Quantity,Price,Active")] OrderDetail orderDetail)
        {
            // Kiểm tra xem id của chi tiết đơn hàng có khớp với OrderDetailId không
            if (id != orderDetail.OrderDetailId)
            {
                return NotFound();
            }
            // Kiểm tra tính hợp lệ của dữ liệu nhập vào
            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin chi tiết đơn hàng và lưu vào cơ sở dữ liệu
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Kiểm tra ngoại lệ đồng thời cập nhật trong cơ sở dữ liệu
                    if (!OrderDetailExists(orderDetail.OrderDetailId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Sau khi cập nhật thành công, chuyển hướng người dùng về trang danh sách (Index)
                return RedirectToAction(nameof(Index));
            }
            // Truyền dữ liệu cần thiết cho View để hiển thị thông tin cần chỉnh sửa
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", orderDetail.OrderId);
            ViewData["ProductId"] = new SelectList(_context.Products, "ProductId", "ProductName", orderDetail.ProductId);
            // Trả về View với thông tin chi tiết đơn hàng để người dùng có thể chỉnh sửa
            return View(orderDetail);
        }

        // GET: Admin/OrderDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderDetail == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Order)
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderDetailId == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: Admin/OrderDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kiểm tra xem bảng OrderDetail có tồn tại hay không
            if (_context.OrderDetail == null)
            {
                return Problem("Entity set 'ShopDBcontext.OrderDetail'  is null.");
            }
            // Tìm chi tiết đơn hàng dựa trên id
            var orderDetail = await _context.OrderDetail.FindAsync(id);
            // Nếu chi tiết đơn hàng tồn tại, xóa nó
            if (orderDetail != null)
            {
                //  xóa 
                _context.OrderDetail.Remove(orderDetail);
            }
            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
            // Chuyển hướng người dùng về trang danh sách (Index)
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
          return (_context.OrderDetail?.Any(e => e.OrderDetailId == id)).GetValueOrDefault();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string searchString)
        {
            // Tạo truy vấn ban đầu cho bảng OrderDetail với thông tin Order và Product tương ứng
            IQueryable<OrderDetail> orderDetailsQuery = _context.OrderDetail.Include(o => o.Order).Include(o => o.Product);
            // Kiểm tra xem chuỗi tìm kiếm có rỗng hoặc null không
            if (!String.IsNullOrEmpty(searchString))
            {
                // Lọc các chi tiết đơn hàng theo các tiêu chí tìm kiếm như ID đơn hàng, tên sản phẩm, số lượng, giá
                orderDetailsQuery = orderDetailsQuery.Where(o =>
                    EF.Functions.Like(o.Order.OrderId.ToString(), $"%{searchString}%") ||
                    EF.Functions.Like(o.Product.ProductName, $"%{searchString}%") ||
                    EF.Functions.Like(o.Quantity.ToString(), $"%{searchString}%") ||
                    EF.Functions.Like(o.Price.ToString(), $"%{searchString}%")
                );
            }

            var searchResults = await orderDetailsQuery.ToListAsync();

            // Truyền kết quả tìm kiếm vào View Index để hiển thị các chi tiết đơn hàng đã lọc
            return View("Index", searchResults);
        }
    }
}
