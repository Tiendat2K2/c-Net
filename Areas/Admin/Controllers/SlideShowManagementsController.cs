using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebBanQuanAo.Models;

namespace WebBanQuanAo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SlideShowManagementsController : Controller
    {
        private readonly ShopDBcontext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SlideShowManagementsController(ShopDBcontext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/SlideShowManagements
        public async Task<IActionResult> Index()
        {
              return _context.SlideShowManagement != null ? 
                          View(await _context.SlideShowManagement.ToListAsync()) :
                          Problem("Entity set 'ShopDBcontext.SlideShowManagement'  is null.");
        }

        // GET: Admin/SlideShowManagements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SlideShowManagement == null)
            {
                return NotFound();
            }

            var slideShowManagement = await _context.SlideShowManagement
                .FirstOrDefaultAsync(m => m.SlideShowId == id);
            if (slideShowManagement == null)
            {
                return NotFound();
            }

            return View(slideShowManagement);
        }

        // GET: Admin/SlideShowManagements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/SlideShowManagements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( SlideShowManagement slideShowManagement)
        {
            if (ModelState.IsValid)
            {
                if (slideShowManagement.ImageFile != null)
                {   // Tạo đường dẫn lưu trữ ảnh
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image/Products");
                    // Tạo tên file độc đáo
                    string uniqueFileName = Path.GetFileNameWithoutExtension(slideShowManagement.ImageFile.FileName) + "_" + Path.GetRandomFileName() + Path.GetExtension(slideShowManagement.ImageFile.FileName);
                    // Tạo đường dẫn đầy đủ đến file ảnh
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // Lưu đường dẫn đến ảnh trong thuộc tính ImagePath của slideShowManagement
                    slideShowManagement.ImagePath = "/Image/Products/" + uniqueFileName;
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        // Lưu file ảnh vào đường dẫn đã xác định
                        await slideShowManagement.ImageFile.CopyToAsync(fileStream);
                    }
                }
                // Thêm chương trình trình diễn mới vào DbContext
                _context.Add(slideShowManagement);
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
                // Chuyển hướng về action "Index" sau khi tạo thành công
                return RedirectToAction(nameof(Index));
            }
            // Hiển thị lại view "Create" nếu dữ liệu không hợp lệ để người dùng có thể nhập lại thông tin
            return View(slideShowManagement);
        }

        // GET: Admin/SlideShowManagements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SlideShowManagement == null)
            {
                return NotFound();
            }

            var slideShowManagement = await _context.SlideShowManagement.FindAsync(id);
            if (slideShowManagement == null)
            {
                return NotFound();
            }
            return View(slideShowManagement);
        }

        // POST: Admin/SlideShowManagements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SlideShowManagement slideShowManagement)
        {
            if (id != slideShowManagement.SlideShowId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (slideShowManagement.ImageFile != null)
                {   // Tạo đường dẫn lưu trữ ảnh
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image/Products");
                    // Tạo tên file độc đáo
                    string uniqueFileName = Path.GetFileNameWithoutExtension(slideShowManagement.ImageFile.FileName) + "_" + Path.GetRandomFileName() + Path.GetExtension(slideShowManagement.ImageFile.FileName);
                    // Tạo đường dẫn đầy đủ đến file ảnh
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // Lưu đường dẫn đến ảnh trong thuộc tính ImagePath của slideShowManagement
                    slideShowManagement.ImagePath = "/Image/Products/" + uniqueFileName;
                    using (var fileStream = new FileStream(filePath, FileMode.Create))

                    {
                        // Lưu file ảnh vào đường dẫn đã xác định
                        await slideShowManagement.ImageFile.CopyToAsync(fileStream);
                    }
                }
                try
                {
                    _context.Update(slideShowManagement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SlideShowManagementExists(slideShowManagement.SlideShowId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(slideShowManagement);
        }

        // GET: Admin/SlideShowManagements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SlideShowManagement == null)
            {
                return NotFound();
            }

            var slideShowManagement = await _context.SlideShowManagement
                .FirstOrDefaultAsync(m => m.SlideShowId == id);
            if (slideShowManagement == null)
            {
                return NotFound();
            }

            return View(slideShowManagement);
        }

        // POST: Admin/SlideShowManagements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SlideShowManagement == null)
            {
                // Kiểm tra nếu không có dữ liệu trong SlideShowManagement
                return Problem("Entity set 'ShopDBcontext.SlideShowManagement'  is null.");
            }
            // Tìm chương trình trình diễn theo id được cung cấp
            var slideShowManagement = await _context.SlideShowManagement.FindAsync(id);
            if (slideShowManagement != null)
            {// Nếu tìm thấy, loại bỏ chương trình trình diễn này khỏi DbContext
                _context.SlideShowManagement.Remove(slideShowManagement);
            }
            // Lưu thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
            // Chuyển hướng về action "Index" sau khi xóa thành công
            return RedirectToAction(nameof(Index));
        }

        private bool SlideShowManagementExists(int id)
        {
          return (_context.SlideShowManagement?.Any(e => e.SlideShowId == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Search(string searchString)
        {
            IQueryable<SlideShowManagement> slideShowsQuery = _context.SlideShowManagement;

            if (!String.IsNullOrEmpty(searchString))
            {
                // Nếu chuỗi tìm kiếm không rỗng, thực hiện truy vấn để tìm các chương trình trình diễn có tiêu đề chứa chuỗi tìm kiếm
                slideShowsQuery = slideShowsQuery.Where(s =>
                // Tìm theo tiêu đề của chương trình trình diễn
                    EF.Functions.Like(s.Title, $"%{searchString}%")
                );
            }
            // Thực hiện truy vấn và chuyển kết quả thành danh sách
            var searchResults = await slideShowsQuery.ToListAsync();

            // Truyền kết quả tìm kiếm vào View "Index" để hiển thị danh sách chương trình trình diễn đã lọc
            return View("Index", searchResults);
        }
    }
}
