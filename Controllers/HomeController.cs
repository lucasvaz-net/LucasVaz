using LucasVaz.Data;
using LucasVaz.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace LucasVaz.Controllers
{
    public class HomeController : Controller
    {
        private readonly PessoaDal _pessoaDal;

        public HomeController(PessoaDal pessoaDal)
        {
            _pessoaDal = pessoaDal;
        }

        public IActionResult Index()
        {
            var pessoa = _pessoaDal.GetPessoa(1);
            return View(pessoa);
        }

    }
}