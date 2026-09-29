using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oganimaster.Models;
using System.Diagnostics;
using X.PagedList;
using X.PagedList.Extensions;
using Oganimaster.ViewModels;
using Oganimaster.Models.Authentication;

namespace Oganimaster.Controllers
{
    public class HomeController : Controller
    {
        // Lưu ý chữ 'L' viết hoa nếu tên file model là QlbanVaLiContext
        QlbanValiContext db = new QlbanValiContext();

        // 1. TRANG CHỦ: Hiển thị toàn bộ sản phẩm + Phân trang
        //[Authentication]
        public IActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            var lstSanPham = db.TDanhMucSps
                               .AsNoTracking()
                               .OrderBy(x => x.TenSp)
                               .ToPagedList(pageNumber, pageSize);

            return View(lstSanPham);
        }

        // 2. TRANG LỌC: Hiển thị sản phẩm theo loại + Phân trang
        public IActionResult SanPhamTheoLoai(string maloai, int? page)
        {
            int pageSize = 8;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            var lstSanPham = db.TDanhMucSps
                               .AsNoTracking()
                               .Where(x => x.MaLoai == maloai)
                               .OrderBy(x => x.TenSp)
                               .ToPagedList(pageNumber, pageSize);

            ViewBag.maloai = maloai;
            return View(lstSanPham);
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

        public IActionResult ChiTietSanPham(string MaSp)
        {
            var sanPham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == MaSp);

            var anhSanPham = db.TAnhSps.Where(x => x.MaSp == MaSp).ToList();
            ViewBag.anhSanPham = anhSanPham;

            return View(sanPham);

        }

        public IActionResult ProductDetail(string MaSp)
        {
            var SanPham = db.TDanhMucSps.SingleOrDefault(x => x.MaSp == MaSp);
            var AnhSanPham = db.TAnhSps.Where(x => x.MaSp == MaSp).ToList(); 
            var homeProductDetailViewModel = new HomeProductDetailViewModel
            {
                danhMucSp = SanPham,
                anhSps = AnhSanPham
            };
            return View(homeProductDetailViewModel);        }
    }
}