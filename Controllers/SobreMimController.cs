using LucasVaz.Data;
using Microsoft.AspNetCore.Mvc;

namespace LucasVaz.Controllers
{
    public class SobreMimController : Controller
    {
        private readonly PessoaDal _pessoaDal;

        public SobreMimController(PessoaDal pessoaDal)
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
