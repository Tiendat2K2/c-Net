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
    public class ShippersController : Controller
    {
        private readonly ShopDBcontext _context;

        public ShippersController(ShopDBcontext context)
        {
            _context = context;
        }

        // GET: Admin/Shippers
        public async Task<IActionResult> Index()
        {
              return _context.Shipper != null ? 
                          View(await _context.Shipper.ToListAsync()) :
                          Problem("Entity set 'ShopDBcontext.Shipper'  is null.");
        }

        // GET: Admin/Shippers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Shipper == null)
            {
                return NotFound();
            }

            var shipper = await _context.Shipper
                .FirstOrDefaultAsync(m => m.ShipperId == id);
            if (shipper == null)
            {
                return NotFound();
            }

            return View(shipper);
        }

        // GET: Admin/Shippers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Shippers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ShipperId,ShipperName,Phone,Active")] Shipper shipper)
        {
            // Kiểm tra xem dữ liệu gửi từ form có hợp lệ không
            if (ModelState.IsValid)
            {
                // Kiểm tra xem số điện thoại đã tồn tại trong cơ sở dữ liệu hay chưa
                var existingShipper = await _context.Shipper.FirstOrDefaultAsync(s => s.Phone == shipper.Phone);

                if (existingShipper != null)
                {
                    // Nếu số điện thoại đã tồn tại, hiển thị thông báo cho người dùng
                    ModelState.AddModelError("Phone", "Số điện thoại đã tồn tại.");
                    return View(shipper);
                }

                // Nếu số điện thoại chưa tồn tại, thêm Shipper mới vào cơ sở dữ liệu
                _context.Add(shipper);
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                // Chuyển hướng về action "Index"
                return RedirectToAction(nameof(Index));
            }
            // Nếu dữ liệu không hợp lệ, hiển thị lại view "Create" để người dùng có thể chỉnh sửa lại thông tin
            return View(shipper);
        }

        // GET: Admin/Shippers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Shipper == null)
            {
                return NotFound();
            }

            var shipper = await _context.Shipper.FindAsync(id);
            if (shipper == null)
            {
                return NotFound();
            }
            return View(shipper);
        }

        // POST: Admin/Shippers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShipperId,ShipperName,Phone,Active")] Shipper shipper)
        {
            // Kiểm tra xem id của Shipper được chỉ định có khớp với id của Shipper gửi từ form không
            if (id != shipper.ShipperId)
            {
                // Trả về lỗi 404 nếu không khớp
                return NotFound();
            }
            // Kiểm tra xem dữ liệu gửi từ form có hợp lệ không
            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin Shipper vào cơ sở dữ liệu và lưu thay đổi
                    _context.Update(shipper);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Xử lý khi có xung đột trong cập nhật dữ liệu
                    if (!ShipperExists(shipper.ShipperId))
                    {
                        // Trả về lỗi 404 nếu không tìm thấy Shipper
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
            return View(shipper);
        }

        // GET: Admin/Shippers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Shipper == null)
            {
                return NotFound();
            }

            var shipper = await _context.Shipper
                .FirstOrDefaultAsync(m => m.ShipperId == id);
            if (shipper == null)
            {
                return NotFound();
            }

            return View(shipper);
        }

        // POST: Admin/Shippers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kiểm tra xem _context.Shipper có tồn tại không
            if (_context.Shipper == null)
            {
                // Trả về lỗi thông báo nếu _context.Shipper không tồn tại
                return Problem("Entity set 'ShopDBcontext.Shipper'  is null.");
            }
            // Tìm Shipper cần xóa từ cơ sở dữ liệu dựa trên ID
            var shipper = await _context.Shipper.FindAsync(id);
            // Kiểm tra xem shipper có tồn tại không
            if (shipper != null)
            {
                // Nếu shipper tồn tại, xóa shipper đó
                _context.Shipper.Remove(shipper);
            }
            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
            // Sau khi xóa xong, chuyển hướng về action "Index" để hiển thị danh sách Shipper
            return RedirectToAction(nameof(Index));
        }

        private bool ShipperExists(int id)
        {
          return (_context.Shipper?.Any(e => e.ShipperId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Search(string searchString)
        {
            // Tạo một đối tượng truy vấn (queryable) để truy vấn dữ liệu từ bảng Shipper trong cơ sở dữ liệu
            IQueryable<Shipper> shippersQuery = _context.Shipper;
            // Kiểm tra xem chuỗi tìm kiếm có giá trị không rỗng
            if (!String.IsNullOrEmpty(searchString))
            {
                shippersQuery = shippersQuery.Where(s =>
                    EF.Functions.Like(s.ShipperName, $"%{searchString}%") ||
                    EF.Functions.Like(s.Phone, $"%{searchString}%")
                );
            }
            // Thực hiện truy vấn và chuyển đổi shippersQuery thành danh sách (List) các Shipper được tìm thấ
            var searchResults = await shippersQuery.ToListAsync();

            // Trả về view "Index" với danh sách các Shipper đã được lọc để hiển thị kết quả tìm kiếm
            return View("Index", searchResults);
        }
    }
}
