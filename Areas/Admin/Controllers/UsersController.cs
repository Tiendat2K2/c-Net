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
    public class UsersController : Controller
    {
        private readonly ShopDBcontext _context;

        public UsersController(ShopDBcontext context)
        {
            _context = context;
        }

        // GET: Admin/Users
        public async Task<IActionResult> Index()
        {     var user = await _context.Users.ToListAsync();
            user.ForEach(x=>x.RoleName=(x.role==1?"Admin":x.role==2?"Sale": "Customer"));
              return _context.Users != null ? 
                          View(user) :
                          Problem("Entity set 'ShopDBcontext.Users'  is null.");
        }

        // GET: Admin/Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Admin/Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserName,Password,Email,role,Active")] User user)
        {   // Kiểm tra xem dữ liệu được gửi từ form có hợp lệ không
            if (ModelState.IsValid)
            {
                // Kiểm tra xem có người dùng nào trong cơ sở dữ liệu đã sử dụng email này chưa
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser != null)
                {
                    // Email đã tồn tại, thông báo lỗi
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                    return View(user);
                }
                else
                {
                    // Nếu email chưa tồn tại, thêm người dùng mới vào cơ sở dữ liệu và chuyển hướng về trang Index
                    _context.Add(user);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            // Nếu dữ liệu không hợp lệ, hiển thị lại form để người dùng nhập thông tin lại
            return View(user);
        }

        // GET: Admin/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserName,Password,Email,role,Active")] User user)
        {
            // Kiểm tra xem id của người dùng được chỉ định có khớp với id của dữ liệu được gửi từ form không
            if (id != user.ID)
            {
                // Nếu không khớp, trả về lỗi 404
                return NotFound();
            }
            // Lấy người dùng hiện tại từ cơ sở dữ liệu dựa trên ID
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.ID == id);

            // Kiểm tra xem dữ liệu gửi từ form có hợp lệ không
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem email đã được sử dụng bởi người dùng khác không
                    var userEmailExists = await _context.Users.AnyAsync(u => u.Email == user.Email && u.ID != id);

                    if (userEmailExists)
                    {
                        // Nếu email đã tồn tại với người dùng khác, thêm thông báo lỗi vào ModelState
                        ModelState.AddModelError("Email", "Email đã tồn tại cho người dùng khác.");
                        return View(user);
                    }
                    // Cập nhật thuộc tính của người dùng hiện tại với dữ liệu từ form
                    existingUser.UserName = user.UserName;
                    existingUser.Password = user.Password;
                    existingUser.Email = user.Email;
                    existingUser.role = user.role;
                    existingUser.Active = user.Active;
                    // Cập nhật thông tin người dùng vào cơ sở dữ liệu và lưu thay đổi
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Xử lý xung đột cập nhật dữ liệu nếu có
                    if (!UserExists(user.ID))
                    {
                        // Nếu người dùng không tồn tại, trả về lỗi 404
                        return NotFound();
                    }
                    else
                    {
                        // Nếu có lỗi khác xảy ra, ném ngoại lệ
                        throw;
                    }
                }
                // Nếu cập nhật thành công, chuyển hướng về trang Index
                return RedirectToAction(nameof(Index));
            }
            // Nếu dữ liệu không hợp lệ, hiển thị lại form để người dùng có thể chỉnh sửa lại thông tin
            return View(user);
        }
        // GET: Admin/Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.ID == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kiểm tra xem `_context.Users` có bằng null không
            if (_context.Users == null)
            {
                // Nếu bằng null, trả về một vấn đề (Problem) thông báo rằng tập hợp 'ShopDBcontext.Users' là null.
                return Problem("Entity set 'ShopDBcontext.Users'  is null.");
            }
            // Tìm kiếm người dùng theo id
            var user = await _context.Users.FindAsync(id);
            // Kiểm tra nếu người dùng tồn tại
            if (user != null)
            {
                // Nếu tồn tại, xóa người dùng khỏi cơ sở dữ liệu
                _context.Users.Remove(user);
            }
            // Lưu các thay đổi vào cơ sở dữ liệu
            await _context.SaveChangesAsync();
            // Chuyển hướng về trang Index sau khi xóa người dùng
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return (_context.Users?.Any(e => e.ID == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> Search(string searchString)
        {
            List<User> searchResults;
            // Kiểm tra xem chuỗi tìm kiếm có giá trị không rỗng
            if (!String.IsNullOrEmpty(searchString))
            {
                // Tạo danh sách searchResults để lưu kết quả tìm kiếm
                searchResults = await _context.Users
                    // Truy vấn cơ sở dữ liệu để lấy người dùng với tên người dùng, mật khẩu hoặc email chứa chuỗi tìm kiếm
                    .Where(u => EF.Functions.Like(u.UserName, $"%{searchString}%")
                             || EF.Functions.Like(u.Password, $"%{searchString}%")
                             || EF.Functions.Like(u.Email, $"%{searchString}%"))
                    .ToListAsync();
            }
            else
            {
                // Nếu chuỗi tìm kiếm rỗng, lấy tất cả người dùng từ cơ sở dữ liệu
                searchResults = await _context.Users.ToListAsync();
            }
            // Trả về view "Index" với danh sách người dùng đã tìm kiếm hoặc toàn bộ danh sách người dùng
            return View("Index", searchResults);
        }
        public async Task<JsonResult> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return Json(users);
        }

    }
}
