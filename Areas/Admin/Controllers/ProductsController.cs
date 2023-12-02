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
    public class ProductsController : Controller
    {
        private readonly ShopDBcontext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(ShopDBcontext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index()
        {
            var shopDBcontext = _context.Products.Include(p => p.Category).Include(p => p.Supplier);
            return View(await shopDBcontext.ToListAsync());
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            // Trong phần này, dữ liệu được chuẩn bị để gửi đến View

            // SelectList được sử dụng để điền dữ liệu vào danh sách thả xuống (dropdown list) trong views
            // Nó chuẩn bị một danh sách các Categories bằng cách chọn CategoryId làm giá trị và CategoryName làm văn bản hiển thị
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            // Một SelectList khác, lần này cho Suppliers
            // Nó chuẩn bị một danh sách các Suppliers bằng cách chọn SupplierId làm giá trị và Address làm văn bản hiển thị
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Address");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid) // Kiểm tra tính hợp lệ của dữ liệu mô hình
            {
                if (product.ImageFile != null) // Kiểm tra xem có tệp tin hình ảnh được tải lên không
                {
                    // Xác định đường dẫn lưu trữ hình ảnh trong thư mục 'images/Category'
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/Category");

                    // Tạo tên file duy nhất dựa trên tên tệp gốc và một số ngẫu nhiên để tránh trùng lặp
                    string uniqueFileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName) + "_" + Path.GetRandomFileName() + Path.GetExtension(product.ImageFile.FileName);

                    // Tạo đường dẫn hoàn chỉnh cho tệp tin hình ảnh
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    // Cập nhật đường dẫn hình ảnh của sản phẩm
                    product.ImagePath = "/images/Category/" + uniqueFileName;

                    // Di chuyển tệp tin hình ảnh vào thư mục chỉ định
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(fileStream);
                    }
                }

                // Thêm sản phẩm vào cơ sở dữ liệu và lưu thay đổi
                _context.Add(product);
                await _context.SaveChangesAsync();

                // Chuyển hướng người dùng đến trang danh sách sản phẩm (Index)
                return RedirectToAction(nameof(Index));
            }

            // Truyền dữ liệu danh mục và nhà cung cấp cho view khi dữ liệu không hợp lệ
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Address", product.SupplierId);

            // Trả về view tạo mới sản phẩm để người dùng có thể sửa lỗi nhập liệu
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Tìm kiếm sản phẩm dựa trên id được cung cấp
            var product = await _context.Products.FindAsync(id);
            // Nếu không tìm thấy sản phẩm với id được cung cấp
            if (product == null)
            {
                return NotFound();
            }
            // Chuẩn bị dữ liệu để điền vào các danh sách thả xuống trong View khi chỉnh sửa sản phẩm
            // Chuẩn bị SelectList cho CategoryId, điền dữ liệu từ _context.Categories
            // CategoryId là giá trị, CategoryName là văn bản hiển thị
            // Đồng thời chọn sản phẩm's CategoryId để hiển thị sản phẩm's category mặc định trong danh sách thả xuống
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            // Tương tự cho SupplierId, chuẩn bị SelectList cho SupplierId
            // SupplierId là giá trị, Address là văn bản hiển thị
            // Đồng thời chọn sản phẩm's SupplierId để hiển thị sản phẩm's supplier mặc định trong danh sách thả xuống
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Address", product.SupplierId);
            // Trả về View và truyền sản phẩm để chỉnh sửa vào View
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {// Kiểm tra nếu id không tồn tại
            if (id != product.ProductId)
            {
                return NotFound();
            }
            // Kiểm tra xem ModelState có hợp lệ không (tức là dữ liệu được nhập vào từ người dùng có đúng hay không)
            if (ModelState.IsValid)
            {
                try
                {
                    // Kiểm tra xem người dùng đã tải lên file hình ảnh mới cho sản phẩm hay chưa
                    if (product.ImageFile != null)
                    {
                        // Lấy đường dẫn đến thư mục lưu trữ hình ảnh trong thư mục wwwroot/images/Category
                        string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/Category");
                        // Tạo tên file duy nhất cho hình ảnh bằng cách kết hợp tên ngẫu nhiên với tên gốc của file
                        string uniqueFileName = Path.GetFileNameWithoutExtension(product.ImageFile.FileName) + "_" + Path.GetRandomFileName() + Path.GetExtension(product.ImageFile.FileName);
                        // Tạo đường dẫn hoàn chỉnh đến file hình ảnh trong thư mục lưu trữ
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        // Cập nhật đường dẫn hình ảnh của sản phẩm trong cơ sở dữ liệu
                        product.ImagePath = "/images/Category/" + uniqueFileName;
                        // Lưu file hình ảnh vào thư mục lưu trữ trên server
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await product.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                    // Cập nhật thông tin sản phẩm trong cơ sở dữ liệu và lưu thay đổi
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Nếu xảy ra lỗi khi cập nhật dữ liệu (có thể do cùng lúc có nhiều người cập nhật), kiểm tra xem sản phẩm có tồn tại hay không
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                // Nếu mọi thứ thành công, chuyển hướng về trang danh sách sản phẩm (Index)
                return RedirectToAction(nameof(Index));
            }
            // Nếu ModelState không hợp lệ, chuẩn bị dữ liệu để điền vào các danh sách thả xuống trong View khi chỉnh sửa sản phẩm
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "SupplierId", "Address", product.SupplierId);
            // Trả về View với thông tin sản phẩm để người dùng có thể chỉnh sửa lại
            return View(product);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ShopDBcontext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> Search(string searchString)

        {
            // Tạo một truy vấn ban đầu chứa tất cả sản phẩm và thông tin về danh mục và nhà cung cấp tương ứng
            IQueryable<Product> productsQuery = _context.Products.Include(p => p.Category).Include(p => p.Supplier);
            // Kiểm tra xem chuỗi tìm kiếm có được cung cấp không và không phải là chuỗi rỗng
            if (!String.IsNullOrEmpty(searchString))
            {
                // Lọc sản phẩm dựa trên chuỗi tìm kiếm vào truy vấn ban đầu
//                tương ứng với câu lệnh sql SELECT p.*
//               FROM Products p
//                LEFT JOIN Categories c ON p.CategoryId = c.CategoryId
//                LEFT JOIN Suppliers s ON p.SupplierId = s.SupplierId
//                WHERE
//                p.ProductName LIKE '%searchString%' OR
//                c.CategoryName LIKE '%searchString%' OR
//                s.SupplierName LIKE '%searchString%' OR
//                p.UnitSize LIKE '%searchString%' OR
//                CAST(p.UnitPrice AS NVARCHAR(MAX)) LIKE '%searchString%' OR
//                p.Description LIKE '%searchString%'
                productsQuery = productsQuery.Where(p =>
                    EF.Functions.Like(p.ProductName, $"%{searchString}%") ||
                    EF.Functions.Like(p.Category.CategoryName, $"%{searchString}%") ||
                    EF.Functions.Like(p.Supplier.SupplierName, $"%{searchString}%") ||
                    EF.Functions.Like(p.UnitSize, $"%{searchString}%") ||
                    EF.Functions.Like(p.UnitPrice.ToString(), $"%{searchString}%") ||
                    EF.Functions.Like(p.Description, $"%{searchString}%")
                );
            }
            // để thực hiện truy vấn đồng bộ hóa danh sách sản phẩm đã được lọc từ productsQuery
            var searchResults = await productsQuery.ToListAsync();

            // Truyền kết quả tìm kiếm vào view "Index" để hiển thị các sản phẩm đã lọc
            return View("Index", searchResults);
        }
    }
}
