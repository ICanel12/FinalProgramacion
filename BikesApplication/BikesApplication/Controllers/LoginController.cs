using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BikesApplication.Models;

namespace BikesApplication.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(string userName, string password)
        {
            BikesContext _BikesApplicationModel = new BikesContext();

            IEnumerable<BikesApplicationModel.User> users = (from u in _BikesApplicationModel.Users
                                                             where u.UserName == userName && u.Password == password
                                                             select new BikesApplicationModel.User
                                                             {
                                                                 UserName = u.UserName,
                                                                 Password = u.Password
                                                             }).ToList();

            if (users.Count() == 1)
            {
                var claims = new List<Claim>();
                claims.Add(new Claim("username", userName));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, password));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(claimsPrincipal);
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Index", "Login");
        }
    }
}
