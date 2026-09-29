using Microsoft.AspNetCore.Mvc;
using Oganimaster.Models;


namespace Oganimaster.Controllers
{
    public class AccessController : Controller
    {
        QlbanValiContext db = new QlbanValiContext();

        [HttpGet]
        public IActionResult Login()
        {
            if(HttpContext.Session.GetString("UserName") == null)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public IActionResult Login(TUser User)
        {
            if (HttpContext.Session.GetString("UserName") == null)
            {
                var u = db.TUsers.FirstOrDefault(x => x.Username == User.Username && x.Password == User.Password);
                if (u != null)
                {
                    HttpContext.Session.SetString("UserName", u.Username.ToString());
                    return RedirectToAction("Index", "Home");

                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Xóa toàn bộ Session và quay về trang Login
            HttpContext.Session.Clear();
            HttpContext.Session.Remove("UserName");
            return RedirectToAction("Login", "Access");
        }
    }
}