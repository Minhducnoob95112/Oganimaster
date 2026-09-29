using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Oganimaster.Models;
using Oganimaster.Models.ProductModels;

namespace Oganimaster.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIController : ControllerBase
    {
        QlbanValiContext db = new QlbanValiContext();

        // 1. GET: api/productapi (Lấy toàn bộ sản phẩm)
        [HttpGet]
        public IEnumerable<Product> GetAllProducts()
        {
            var sanPhams = (from p in db.TDanhMucSps
                            select new Product
                            {
                                MaSp = p.MaSp,
                                TenSp = p.TenSp,
                                MaLoai = p.MaLoai,
                                AnhDaiDien = p.AnhDaiDien,
                                GiaNhoNhat = p.GiaNhoNhat
                            }).ToList();

            return sanPhams;
        }

        // 2. GET: api/productapi/{maloai} (Lấy sản phẩm theo mã loại)
        [HttpGet("{maloai}")]
        public IEnumerable<Product> GetProductsByCategory(string maloai)
        {
            var sanPhams = (from p in db.TDanhMucSps
                            where p.MaLoai == maloai
                            select new Product
                            {
                                MaSp = p.MaSp,
                                TenSp = p.TenSp,
                                MaLoai = p.MaLoai,
                                AnhDaiDien = p.AnhDaiDien,
                                GiaNhoNhat = p.GiaNhoNhat
                            }).ToList();

            return sanPhams;
        }

    }
}