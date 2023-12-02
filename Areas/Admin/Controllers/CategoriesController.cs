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
    public class CategoriesController : Controller
    {
        private readonly ShopDBcontext _context;

        public CategoriesController(ShopDBcontext context)
        {
            _context = context;
        }

        // GET: Admin/Categories
        public async Task<IActionResult> Index()
        {
            return _context.Categories != null ?
                        View(await _context.Categories.ToListAsync()) :
                        Problem("Entity set 'ShopDBcontext.Categories'  is null.");
        }

        // GET: Admin/Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Admin/Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] // Xác định rằng phương thức này xử lý các yêu cầu HTTP POST từ client.
        [ValidateAntiForgeryToken] // Bảo vệ ứng dụng khỏi tấn công CSRF bằng cách kiểm tra token từ form với token trên server.
        public async Task<IActionResult> Create([Bind("CategoryId,CategoryName,Active")] Category category)
        {
            if (ModelState.IsValid) // Kiểm tra xem trạng thái của model có hợp lệ không sau khi đã binding và kiểm tra dữ liệu.
            {
                _context.Add(category); // Thêm đối tượng category vào đối tượng context (_context) của database nhưng chưa lưu vào database.
                await _context.SaveChangesAsync(); // Lưu thay đổi vào database một cách bất đồng bộ.
                return RedirectToAction(nameof(Index)); // Nếu dữ liệu hợp lệ và đã thêm thành công category vào database, chuyển hướng người dùng đến action 'Index' của controller hiện tại.
            }
            return View(category); // Nếu dữ liệu không hợp lệ (ví dụ: lỗi kiểm tra dữ liệu), trả về view 'Create' với thông tin category để hiển thị lỗi và cho phép người dùng chỉnh sửa.
        }

        // GET: Admin/Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Admin/Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] // Xác định phương thức sẽ xử lý yêu cầu HTTP POST từ người dùng.
        [ValidateAntiForgeryToken] // Bảo vệ ứng dụng chống lại CSRF bằng cách xác minh tính hợp lệ của mã thông báo trong yêu cầu POST.

        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Active")] Category category)
        {
            if (id != category.CategoryId) // Kiểm tra xem ID từ đường dẫn có khớp với ID của đối tượng category được chỉnh sửa hay không.
            {
                return NotFound(); // Trả về trạng thái 404 Not Found nếu không khớp ID.
            }

            if (ModelState.IsValid) // Kiểm tra xem dữ liệu nhập vào từ form có hợp lệ hay không, dựa trên các quy tắc được xác định trong model.
            {
                try
                {
                    _context.Update(category); // Cập nhật đối tượng category trong context.
                    await _context.SaveChangesAsync(); // Lưu các thay đổi vào cơ sở dữ liệu.
                }
                catch (DbUpdateConcurrencyException) // Xử lý ngoại lệ khi cố gắng cập nhật cơ sở dữ liệu.
                {
                    if (!CategoryExists(category.CategoryId)) // Kiểm tra xem danh mục có tồn tại không.
                    {
                        return NotFound(); // Trả về trạng thái 404 Not Found nếu không tồn tại.
                    }
                    else
                    {
                        throw; // Ném ngoại lệ để xử lý ở mức cao hơn nếu có lỗi khác xảy ra.
                    }
                }
                return RedirectToAction(nameof(Index)); // Chuyển hướng người dùng đến action Index của controller hiện tại sau khi chỉnh sửa thành công.
            }
            return View(category); // Nếu dữ liệu không hợp lệ, trả về lại view của Edit với đối tượng category để người dùng có thể sửa các lỗi nhập liệu.
        }

        // GET: Admin/Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _context.Categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Admin/Categories/Delete/5
        [HttpPost, ActionName("Delete")] // Xác định phương thức sẽ xử lý yêu cầu HTTP POST với tên hành động là "Delete".
        [ValidateAntiForgeryToken] // Bảo vệ ứng dụng chống lại CSRF bằng cách xác minh tính hợp lệ của mã thông báo trong yêu cầu POST.

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null) // Kiểm tra xem tập hợp (entity set) Categories trong context có null không.
            {
                return Problem("Entity set 'ShopDBcontext.Categories' is null."); // Trả về lỗi nếu tập hợp là null.
            }

            var category = await _context.Categories.FindAsync(id); // Tìm đối tượng danh mục trong cơ sở dữ liệu dựa trên ID.

            if (category != null) // Nếu đối tượng danh mục tồn tại trong cơ sở dữ liệu.
            {
                _context.Categories.Remove(category); // Xóa đối tượng danh mục khỏi tập hợp Categories.
            }

            await _context.SaveChangesAsync(); // Lưu các thay đổi vào cơ sở dữ liệu.
            return RedirectToAction(nameof(Index)); // Chuyển hướng người dùng đến action Index của controller hiện tại sau khi xóa thành công.
        }

        private bool CategoryExists(int id)
        {
            return (_context.Categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Search(string searchString)
        {
            IQueryable<Category> categoriesQuery = _context.Categories; // Tạo một truy vấn IQueryable ban đầu từ tập hợp Categories trong context.

            if (!String.IsNullOrEmpty(searchString)) // Kiểm tra xem chuỗi tìm kiếm có giá trị không rỗng hoặc null.
            {
                // Nếu chuỗi tìm kiếm không rỗng, thêm điều kiện vào truy vấn để lọc các danh mục theo tên chứa chuỗi tìm kiếm.
                // ứng  với câu lệnh SELECT* FROM Categories WHERE CategoryName LIKE '%searchString%';
                categoriesQuery = categoriesQuery.Where(c => EF.Functions.Like(c.CategoryName, $"%{searchString}%"));
                // Dùng hàm EF.Functions.Like để thực hiện tìm kiếm linh hoạt theo mẫu trong cột CategoryName.
            }

            var searchResults = await categoriesQuery.ToListAsync(); // Thực hiện truy vấn và lấy kết quả tìm kiếm danh sách danh mục.

            // Chuyển kết quả tìm kiếm đến view "Index" để hiển thị các danh mục đã lọc.
            return View("Index", searchResults);
        }
    }
}
    

