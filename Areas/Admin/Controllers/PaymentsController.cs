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
    public class PaymentsController : Controller
    {
        private readonly ShopDBcontext _context;

        public PaymentsController(ShopDBcontext context)
        {
            _context = context;
        }

        // GET: Admin/Payments
        public async Task<IActionResult> Index()
        {
            var shopDBcontext = _context.Payment.Include(p => p.Order);
            return View(await shopDBcontext.ToListAsync());
        }

        // GET: Admin/Payments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Payment == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .Include(p => p.Order)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }

            return View(payment);
        }

        // GET: Admin/Payments/Create
        public IActionResult Create()
        {   // Truyền dữ liệu OrderId đến view để người dùng có thể chọn từ danh sách OrderId có sẵn
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId");
            // Trả về view Create, nơi mà người dùng có thể tạo mới một đối tượng
            return View();
        }

        // POST: Admin/Payments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PaymentId,OrderId,PaymentDate,AmountPaid,PaymentMethod,Active")] Payment payment)
        {
            // Kiểm tra xem dữ liệu nhập vào có hợp lệ không
            if (ModelState.IsValid)
            {
                // Thêm đối tượng Payment vào cơ sở dữ liệu và lưu thay đổi
                _context.Add(payment);
                await _context.SaveChangesAsync();
                // Sau khi thêm thành công, chuyển hướng người dùng đến trang danh sách Payment (Index)
                return RedirectToAction(nameof(Index));
            }
            // Nếu dữ liệu không hợp lệ, truyền lại OrderId và hiển thị view Create với dữ liệu đã nhập
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", payment.OrderId);
            return View(payment);
        }

        // GET: Admin/Payments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            // Kiểm tra xem id có tồn tại và _context.Payment có dữ liệu hay không
            if (id == null || _context.Payment == null)
            {
                // Nếu không, trả về trang 404 Not Found
                return NotFound();
            }
            // Tìm đối tượng Payment trong cơ sở dữ liệu dựa trên id
            var payment = await _context.Payment.FindAsync(id);
            if (payment == null)
            {
                // Nếu không tìm thấy đối tượng Payment tương ứng với id, trả về trang 404 Not Found
                return NotFound();
            }
            // Truyền danh sách OrderId vào view để người dùng có thể chọn từ dropdown list khi chỉnh sửa Payment
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", payment.OrderId);
            // Trả về view Edit với dữ liệu của đối tượng Payment để người dùng có thể chỉnh sửa
            return View(payment);
        }

        // POST: Admin/Payments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PaymentId,OrderId,PaymentDate,AmountPaid,PaymentMethod,Active")] Payment payment)
        {
            // Kiểm tra xem id của payment có khớp với id truyền vào không
            if (id != payment.PaymentId)
            {
                // Nếu không khớp, trả về trang 404 Not Found
                return NotFound();
            }
            // Kiểm tra tính hợp lệ của dữ liệu nhập vào từ form
            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin của payment trong cơ sở dữ liệu và lưu thay đổi
                    _context.Update(payment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Nếu xảy ra lỗi khi cập nhật, kiểm tra xem payment có tồn tại không
                    if (!PaymentExists(payment.PaymentId))
                    {
                        // Nếu không tồn tại, trả về trang 404 Not Found
                        return NotFound();
                    }
                    else
                    {
                        // Nếu tồn tại, ngoại lệ sẽ được ném ra để xử lý tiếp
                        throw;
                    }
                }
                // Nếu cập nhật thành công, chuyển hướng người dùng về trang danh sách Payment (Index)
                return RedirectToAction(nameof(Index));
            }
            // Truyền danh sách OrderId vào view để người dùng có thể chọn từ dropdown list khi chỉnh sửa Payment
            ViewData["OrderId"] = new SelectList(_context.Order, "OrderId", "OrderId", payment.OrderId);
            // Trả về view Edit với dữ liệu của payment để người dùng có thể chỉnh sửa
            return View(payment);
        }

        // GET: Admin/Payments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Payment == null)
            {
                return NotFound();
            }

            var payment = await _context.Payment
                .Include(p => p.Order)
                .FirstOrDefaultAsync(m => m.PaymentId == id);
            if (payment == null)
            {
                return NotFound();
            }
            // Trả về View để người dùng có thể chỉnh sửa thông tin của payment
            return View(payment);
        }

        // POST: Admin/Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kiểm tra xem tập hợp Entity Payment có tồn tại không
            if (_context.Payment == null)
            {// Nếu tập hợp không tồn tại, trả về một thông báo lỗi
                return Problem("Entity set 'ShopDBcontext.Payment'  is null.");
            }
            // Tìm khoản thanh toán có Id trùng khớp với Id được chỉ định
            var payment = await _context.Payment.FindAsync(id);
            // Nếu khoản thanh toán tồn tại
            if (payment != null)
            {
                // Xóa khoản thanh toán khỏi tập hợp
                _context.Payment.Remove(payment);
            }
            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
            // Chuyển hướng về trang Index sau khi xóa thành công
            return RedirectToAction(nameof(Index));
        }

        private bool PaymentExists(int id)
        {
          return (_context.Payment?.Any(e => e.PaymentId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Search(string searchString)
        {
            // Tạo một truy vấn linh hoạt để lấy thông tin về các khoản thanh toán, bao gồm thông tin đơn hàng liên quan
            IQueryable<Payment> paymentsQuery = _context.Payment.Include(p => p.Order);
            // Kiểm tra xem chuỗi tìm kiếm có giá trị không
            if (!String.IsNullOrEmpty(searchString))
            {// Thực hiện tìm kiếm trong các thuộc tính cụ thể của các khoản thanh toán
                paymentsQuery = paymentsQuery.Where(p =>
                // Tìm kiếm theo OrderId
                    EF.Functions.Like(p.Order.OrderId.ToString(), $"%{searchString}%") ||
                    // Tìm kiếm theo ngày thanh toán
                    EF.Functions.Like(p.PaymentDate.ToString(), $"%{searchString}%") ||
                    // Tìm kiếm theo số tiền đã thanh toán
                    EF.Functions.Like(p.AmountPaid.ToString(), $"%{searchString}%") ||
                    // Tìm kiếm theo phương thức thanh toán
                    EF.Functions.Like(p.PaymentMethod, $"%{searchString}%")
                );
            }
            // Thực hiện truy vấn và chuyển kết quả tìm kiếm thành một danh sách các khoản thanh toán
            var searchResults = await paymentsQuery.ToListAsync();

            // Trả về view "Index" và truyền danh sách các khoản thanh toán đã lọc để hiển thị
            return View("Index", searchResults);
        }
    }
}
