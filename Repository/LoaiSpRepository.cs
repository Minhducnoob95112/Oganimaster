using Oganimaster.Models;

namespace Oganimaster.Repository
{
    public class LoaiSpRepository : ILoaiSpRepository
    {
        private readonly QlbanValiContext _context;

        public LoaiSpRepository(QlbanValiContext context)
        {
            _context = context;
        }

        public TLoaiSp Add(TLoaiSp loaiSp)
        {
            _context.TLoaiSps.Add(loaiSp);
            _context.SaveChanges();
            return loaiSp;
        }

        public TLoaiSp Update(TLoaiSp loaiSp)
        {
            _context.Update(loaiSp);
            _context.SaveChanges();
            return loaiSp;
        }

        public TLoaiSp Delete(string maloaiSp)
        {
            var loaiSp = _context.TLoaiSps.Find(maloaiSp);
            if (loaiSp != null)
            {
                _context.TLoaiSps.Remove(loaiSp);
                _context.SaveChanges();
            }
            return loaiSp;
        }

        public TLoaiSp GetLoaiSp(string maloaiSp)
        {
            return _context.TLoaiSps.Find(maloaiSp);
        }

        public IEnumerable<TLoaiSp> GetAllLoaiSp()
        {
            return _context.TLoaiSps.OrderBy(x => x.Loai);
        }
    }
}