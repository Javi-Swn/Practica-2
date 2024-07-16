using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Practica_2.Models;
using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Practica_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ListasDbContext _context;

        public HomeController(ILogger<HomeController> logger, ListasDbContext context)
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

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [Authorize]
        public IActionResult Dash()
        {
            var userIdClaim = User.FindFirst("UserId");
            if (userIdClaim == null)
            {
                return Unauthorized();
            }
            var usuarioId = int.Parse(userIdClaim.Value);

            var listas = _context.ListasDeReproduccions
                                .Where(l => l.UsuarioId == usuarioId)
                                .ToList();

            return View(listas);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitForm(Usuario usuario)
        {
            if (usuario != null)
            {
                _context.Add(usuario);
                await _context.SaveChangesAsync();
                return View("Login");
            }

            return Content("<a>Error en la operación</a>");
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Correo, string password)
        {
            var user = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == Correo && u.Contraseña == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Nombre),
                    new Claim(ClaimTypes.Email, user.Correo),
                    new Claim("UserId", user.UsuarioId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return RedirectToAction("Dash");
            }

            ViewBag.ErrorMessage = "Usuario o contraseña incorrectos";
            return View("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
