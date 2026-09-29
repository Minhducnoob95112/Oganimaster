using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Oganimaster.Models;
using X.PagedList;
using X.PagedList.Extensions;

namespace Oganimaster.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin")]
    [Route("admin/home")]
    public class HomeController : Controller
    {
        QlbanValiContext db = new QlbanValiContext();

        // 1. TRANG CHỦ ADMIN (Chỉ DUY NHẤT Action này được nhận route "")
        [Route("")]
        [Route("index")]
        public IActionResult Index()
        {
            return View();
        }

        // 2. DANH MỤC SẢN PHẨM (Bài 9)
        [Route("danhmucsanpham")]
        public IActionResult DanhMucSanPham(int? page)
        {
            int pageSize = 12;
            int pageNumber = page == null || page < 0 ? 1 : page.Value;

            var lstSanPham = db.TDanhMucSps
                               .AsNoTracking()
                               .OrderBy(x => x.TenSp)
                               .ToPagedList(pageNumber, pageSize);

            return View(lstSanPham);
        }

        // 3. THÊM SẢN PHẨM MỚI - GET (Bài 10)
        [Route("themsanphammoi")]
        [HttpGet]
        public IActionResult ThemSanPhamMoi()
        {
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");

            return View();
        }

        // 3. THÊM SẢN PHẨM MỚI - POST (Bài 10)
        [Route("themsanphammoi")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ThemSanPhamMoi(TDanhMucSp sanPham)
        {
            if (ModelState.IsValid)
            {
                db.TDanhMucSps.Add(sanPham);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham");
            }

            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");

            return View(sanPham);
        }

        // 4. CHỈNH SỬA SẢN PHẨM - GET (Bài 11)
        [Route("suasanpham")]
        [HttpGet]
        public IActionResult SuaSanPham(string maSanPham)
        {
            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");

            var sanPham = db.TDanhMucSps.Find(maSanPham);
            return View(sanPham);
        }

        // 4. CHỈNH SỬA SẢN PHẨM - POST (Bài 11)
        [Route("suasanpham")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SuaSanPham(TDanhMucSp sanPham)
        {
            if (ModelState.IsValid)
            {
                db.Update(sanPham);
                db.SaveChanges();
                return RedirectToAction("DanhMucSanPham");
            }

            ViewBag.MaChatLieu = new SelectList(db.TChatLieus.ToList(), "MaChatLieu", "ChatLieu");
            ViewBag.MaHangSx = new SelectList(db.THangSxes.ToList(), "MaHangSx", "HangSx");
            ViewBag.MaNuocSx = new SelectList(db.TQuocGia.ToList(), "MaNuoc", "TenNuoc");
            ViewBag.MaLoai = new SelectList(db.TLoaiSps.ToList(), "MaLoai", "Loai");
            ViewBag.MaDt = new SelectList(db.TLoaiDts.ToList(), "MaDt", "TenLoai");

            return View(sanPham);
        }

        // 5. XÓA SẢN PHẨM (Bài 12)
        [Route("xoasanpham")]
        [HttpGet]
        public IActionResult XoaSanPham(string maSanPham)
        {
            // Kiểm tra xem sản phẩm đã có chi tiết sản phẩm / hóa đơn phát sinh chưa
            var chiTietSanPhams = db.TChiTietSanPhams.Where(x => x.MaSp == maSanPham).ToList();
            if (chiTietSanPhams.Count() > 0)
            {
                TempData["Message"] = "Không xóa được sản phẩm này vì đã có chi tiết sản phẩm / hóa đơn!";
                return RedirectToAction("DanhMucSanPham");
            }

            // Xóa các ảnh liên quan trong bảng TAnhSp trước
            var anhSanPhams = db.TAnhSps.Where(x => x.MaSp == maSanPham).ToList();
            if (anhSanPhams.Any())
            {
                db.RemoveRange(anhSanPhams);
            }

            // Xóa bản ghi trong TDanhMucSp
            var sanPham = db.TDanhMucSps.Find(maSanPham);
            if (sanPham != null)
            {
                db.Remove(sanPham);
                db.SaveChanges();
                TempData["Message"] = "Sản phẩm đã được xóa thành công!";
            }

            return RedirectToAction("DanhMucSanPham");
        }
    }
}