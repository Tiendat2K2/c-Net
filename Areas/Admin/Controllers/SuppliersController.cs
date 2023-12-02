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
    public class SuppliersController : Controller
    {
        private readonly ShopDBcontext _context;

        public SuppliersController(ShopDBcontext context)
        {
            _context = context;
        }

        // GET: Admin/Suppliers
        public async Task<IActionResult> Index()
        {
              return _context.Suppliers != null ? 
                          View(await _context.Suppliers.ToListAsync()) :
                          Problem("Entity set 'ShopDBcontext.Suppliers'  is null.");
        }

        // GET: Admin/Suppliers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierId == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // GET: Admin/Suppliers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Suppliers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SupplierId,SupplierName,Address,Phone,Active")] Supplier supplier)
        {
            // Kiểm tra xem dữ liệu gửi lên từ form có hợp lệ không, dựa trên các quy tắc xác định trong ModelState
            if (ModelState.IsValid)
            {
                // Thêm nhà cung cấp mới vào DbContext
                _context.Add(supplier);
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                // Chuyển hướng về action "Index" sau khi tạo thành công
                return RedirectToAction(nameof(Index));
            }
            // Hiển thị lại view "Create" nếu dữ liệu không hợp lệ để người dùng có thể nhập lại thông tin
            return View(supplier);
        }

        // GET: Admin/Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return View(supplier);
        }

        // POST: Admin/Suppliers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SupplierId,SupplierName,Address,Phone,Active")] Supplier supplier)
        {
            // Kiểm tra xem id của nhà cung cấp có khớp với SupplierId của dữ liệu được gửi từ form không
            if (id != supplier.SupplierId)
            {
                return NotFound();
            }
            // Kiểm tra xem ModelState có chứa dữ liệu hợp lệ không, tức là dữ liệu được gửi từ form đáp ứng các yêu cầu của mô hình dữ liệu
            if (ModelState.IsValid)
            {
                try
                {
                    // Cập nhật thông tin của nhà cung cấp vào cơ sở dữ liệu và lưu thay đổi
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Xử lý khi có xung đột trong cập nhật dữ liệu
                    if (!SupplierExists(supplier.SupplierId))
                    {
                        // Trả về NotFound nếu không tìm thấy nhà cung cấp
                        return NotFound();
                    }
                    else
                    {
                        // Ném ngoại lệ nếu có lỗi khác xảy ra
                        throw;
                    }
                }
                // Chuyển hướng về action "Index" nếu cập nhật thành công
                return RedirectToAction(nameof(Index));
            }
            // Hiển thị lại view "Edit" nếu dữ liệu không hợp lệ để người dùng có thể chỉnh sửa lại thông tin
            return View(supplier);
        }

        // GET: Admin/Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Suppliers == null)
            {
                return NotFound();
            }

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(m => m.SupplierId == id);
            if (supplier == null)
            {
                return NotFound();
            }

            return View(supplier);
        }

        // POST: Admin/Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Suppliers == null)
            {
                // Trả về thông báo lỗi nếu tập hợp không tồn tại
            return Problem("Entity set 'ShopDBcontext.Suppliers'  is null.");
            }
            var supplier = await _context.Suppliers.FindAsync(id);
            // Nếu nhà cung cấp được tìm thấy
            if (supplier != null)
            {
                // Xóa nhà cung cấp khỏi DbContext
                _context.Suppliers.Remove(supplier);
            }
            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
            // Chuyển hướng về action "Index" sau khi xóa thành công
            return RedirectToAction(nameof(Index));
        }

        private bool SupplierExists(int id)
        {
          return (_context.Suppliers?.Any(e => e.SupplierId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Search(string searchString)
        {
            // Tạo truy vấn ban đầu từ bảng Suppliers
            IQueryable<Supplier> suppliersQuery = _context.Suppliers;

            if (!String.IsNullOrEmpty(searchString))
            {// Nếu chuỗi tìm kiếm không rỗng
                suppliersQuery = suppliersQuery.Where(s =>
                // Tìm theo tên nhà cung cấp
                    EF.Functions.Like(s.SupplierName, $"%{searchString}%") ||
                    // Tìm theo địa chỉ
                    EF.Functions.Like(s.Address, $"%{searchString}%") ||
                    // Tìm theo số điện thoại
                    EF.Functions.Like(s.Phone, $"%{searchString}%")
                );
            }
            // Thực hiện truy vấn và chuyển kết quả thành danh sách
            var searchResults = await suppliersQuery.ToListAsync();

            // Truyền kết quả tìm kiếm vào View "Index" để hiển thị các nhà cung cấp đã lọc
            return View("Index", searchResults);
        }
    }
}
