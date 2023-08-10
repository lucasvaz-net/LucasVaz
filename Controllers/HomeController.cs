using LucasVaz.Data;
using Microsoft.AspNetCore.Mvc;


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