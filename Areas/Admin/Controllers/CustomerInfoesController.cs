using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanQuanAo.Models;

namespace WebBanQuanAo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerInfoesController : Controller
    {
        private readonly ShopDBcontext _context;

        public CustomerInfoesController(ShopDBcontext context)
        {
            _context = context;
        }

        // GET: Admin/CustomerInfoes
        public async Task<IActionResult> Index()
        {
            return _context.CustomerInfo != null ?
                        View(await _context.CustomerInfo.ToListAsync()) :
                        Problem("Entity set 'ShopDBcontext.CustomerInfo'  is null.");
        }

        // GET: Admin/CustomerInfoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.CustomerInfo == null)
            {
                return NotFound();
            }

            var customerInfo = await _context.CustomerInfo
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customerInfo == null)
            {
                return NotFound();
            }

            return View(customerInfo);
        }

        // GET: Admin/CustomerInfoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/CustomerInfoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustomerId,CustomerName,Address,Phone,Active")] CustomerInfo customerInfo)
        {
            // Kiểm tra xem dữ liệu gửi từ form có hợp lệ không
            if (ModelState.IsValid)
            {// Thêm đối tượng CustomerInfo vào DbContext
                _context.Add(customerInfo);
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                // Chuyển hướng người dùng về action "Index" sau khi thêm thành công
                return RedirectToAction(nameof(Index));
            }
            // Nếu dữ liệu không hợp lệ, hiển thị lại view "Create" để người dùng có thể chỉnh sửa lại thông tin
            return View(customerInfo);
        }

        // GET: Admin/CustomerInfoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.CustomerInfo == null)
            {
                return NotFound();
            }

            var customerInfo = await _context.CustomerInfo.FindAsync(id);
            if (customerInfo == null)
            {
                return NotFound();
            }


            return View(customerInfo);
        }

        // POST: Admin/CustomerInfoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustomerId,CustomerName,Address,Phone,Active")] CustomerInfo customerInfo)
        {
            // Kiểm tra xem id của CustomerInfo truyền vào có khớp với CustomerId được chỉ định không
            if (id != customerInfo.CustomerId)
            {
                // Trả về NotFound nếu không khớp
                return NotFound();
            }
            // Kiểm tra xem dữ liệu gửi từ form có hợp lệ không
            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin của CustomerInfo vào cơ sở dữ liệu và lưu thay đổi
                    _context.Update(customerInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Xử lý khi có xung đột trong cập nhật dữ liệu
                    if (!CustomerInfoExists(customerInfo.CustomerId))
                    {
                        // Trả về NotFound nếu không tìm thấy CustomerInfo
                        return NotFound();
                    }
                    else
                    {
                        // Ném ngoại lệ nếu có lỗi khác xảy ra
                        throw;
                    }
                }
                // Chuyển hướng về action "Index"
                return RedirectToAction(nameof(Index));
            }
            // Hiển thị lại view "Edit" nếu dữ liệu không hợp lệ để người dùng có thể chỉnh sửa lại thông tin
            return View(customerInfo);
        }

        // GET: Admin/CustomerInfoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.CustomerInfo == null)
            {
                return NotFound();
            }

            var customerInfo = await _context.CustomerInfo
                .FirstOrDefaultAsync(m => m.CustomerId == id);
            if (customerInfo == null)
            {
                return NotFound();
            }

            return View(customerInfo);
        }

        // POST: Admin/CustomerInfoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kiểm tra xem tập hợp Entity CustomerInfo có tồn tại không
            if (_context.CustomerInfo == null)
            {
                return Problem("Entity set 'ShopDBcontext.CustomerInfo'  is null.");
            }
            // Tìm CustomerInfo có CustomerId tương ứng với id được cung cấp
            var customerInfo = await _context.CustomerInfo.FindAsync(id);
            // Nếu CustomerInfo được tìm thấy
            if (customerInfo != null)
            {
                // Xóa CustomerInfo khỏi DbContext
                _context.CustomerInfo.Remove(customerInfo);
            }
            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
            // Chuyển hướng về action "Index" sau khi xóa thành công
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerInfoExists(int id)
        {
            return (_context.CustomerInfo?.Any(e => e.CustomerId == id)).GetValueOrDefault();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string searchString)
        {
            // Khởi tạo danh sách kết quả tìm kiếm
            List<CustomerInfo> searchResults;

            if (!String.IsNullOrEmpty(searchString))
            {
                // Nếu chuỗi tìm kiếm không rỗng
                searchResults = await _context.CustomerInfo
                    .Where(c =>
                    // Tìm theo tên khách hàng
                        EF.Functions.Like(c.CustomerName, $"%{searchString}%") ||
                        //Tìm theo địa chỉ
                        EF.Functions.Like(c.Address, $"%{searchString}%") ||
                        // Tìm theo số điện thoại
                        EF.Functions.Like(c.Phone, $"%{searchString}%")
                    )
                    // Chuyển kết quả tìm kiếm thành danh sách
                    .ToListAsync();
            }
            else
            {
                // Nếu chuỗi tìm kiếm rỗng, lấy tất cả bản ghi từ cơ sở dữ liệu
                searchResults = await _context.CustomerInfo.ToListAsync();
            }
            // Trả về view "Index" với danh sách kết quả tìm kiếm hoặc toàn bộ danh sách nếu không có tìm kiếm
            return View("Index", searchResults);
        }

    }
}
