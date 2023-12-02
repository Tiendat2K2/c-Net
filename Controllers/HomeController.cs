using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebBanQuanAo.Models;

namespace WebBanQuanAo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ShopDBcontext _context;

        public HomeController(ILogger<HomeController> logger, ShopDBcontext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid) // Kiểm tra tính hợp lệ của dữ liệu được gửi từ form
            {
                // Kiểm tra xem đã có người dùng nào khác sử dụng email này chưa
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);

                if (existingUser != null) // Nếu đã tồn tại người dùng với email này
                {
                    // Thêm lỗi vào ModelState để hiển thị thông báo lỗi trong form
                    ModelState.AddModelError("Email", "Email đã tồn tại.");
                    return View(user); // Hiển thị view Register với thông báo lỗi
                }

                // Nếu email chưa tồn tại, thêm người dùng mới vào cơ sở dữ liệu
                _context.Add(user);
                await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu
                return RedirectToAction(nameof(Index)); // Chuyển hướng đến action Index
            }

            // Nếu dữ liệu không hợp lệ, cấp vai trò mặc định cho người dùng và hiển thị form
            user.role = 3; // Gán vai trò mặc định
            return View(user); // Hiển thị form Register để người dùng nhập thông tin lại
        }
        public IActionResult Login()
        {
            return View();
        }
        // Thuộc tính này giúp ngăn chặn các cuộc tấn công giả mạo yêu cầu từ các trang web khác (CSRF).
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Login login)
        {
            if (ModelState.IsValid) // Kiểm tra xem trạng thái của mô hình có hợp lệ không
            {
                // Tìm kiếm người dùng trong cơ sở dữ liệu dựa trên tên đăng nhập và mật khẩu được cung cấp
                var user = _context.Users.FirstOrDefault(x => x.UserName == login.Username && x.Password == login.Password);

                if (user != null && user.ID > 0) // Nếu tìm thấy người dùng và có ID hợp lệ
                {
                    if (user.role == 1 || user.role == 2) // Kiểm tra vai trò của người dùng để điều hướng cho Âdmin
                    {
                        // Chuyển hướng đến trang chủ khu vực Admin
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else
                    {
                        // Chuyển hướng đến trang chủ của người dùng thông thường
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    // Nếu không tìm thấy người dùng hoặc không hợp lệ, chuyển hướng đến trang đăng nhập lại
                    return RedirectToAction("Login", "Home");
                }
            }
            // Trả về view với mô hình đăng nhập nếu trạng thái mô hình không hợp lệ
            return View(login);
        }
        public IActionResult Forgot_password()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Forgot_password(User model)
        {
            // Lấy người dùng từ cơ sở dữ liệu dựa trên địa chỉ email được cung cấp trong đối tượng User model
            var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user != null)
            {
                // Nếu tìm thấy người dùng với địa chỉ email này trong cơ sở dữ liệu

                // Cập nhật mật khẩu của người dùng thành mật khẩu mới được cung cấp trong đối tượng User model
                user.Password = model.Password;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.SaveChanges();

                // Chuyển hướng người dùng đến hành động "Index" trong controller hiện tại
                // Điều này có thể là trang chủ hoặc trang khác, tùy thuộc vào cách bạn cấu hình ứng dụng
                return RedirectToAction("Index");
            }
            else
            {
                // Nếu không tìm thấy người dùng với địa chỉ email trong cơ sở dữ liệu

                // Thêm lỗi vào ModelState để hiển thị thông báo lỗi trên giao diện người dùng
                ModelState.AddModelError("Email", "Email không tồn tại");

                // Trả về view "Forgot_password" với model ban đầu (để hiển thị thông tin người dùng nhập và thông báo lỗi)
                return View(model);
            }
        }



    }
    }
    
