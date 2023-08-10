using LucasVaz.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Collections.Generic;

namespace LucasVaz.Controllers
{
    public class LoginController : Controller
    {
        private readonly PessoaDal _pessoaDal;

        public LoginController(PessoaDal pessoaDal)
        {
            _pessoaDal = pessoaDal;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                ModelState.AddModelError("", "Por favor, forneça o email e senha.");
                return View();
            }

            int? userId = _pessoaDal.GetPessoaLogin(email, password);
            if (userId.HasValue)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString())
                    // Aqui, você pode adicionar mais claims se necessário.
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Home", "Login");
            }
            else
            {
                ModelState.AddModelError("", "Login inválido. Verifique o email e a senha fornecidos.");
                return View();
            }
        }

        [Authorize]
        public IActionResult Home()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Home", "Login");
        }



        [HttpGet]
        public IActionResult AcessoNegado()
        {
            return View();
        }
    }
}
