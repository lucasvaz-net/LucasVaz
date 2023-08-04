using LucasVaz.Data;
using Microsoft.AspNetCore.Mvc;

namespace LucasVaz.Controllers
{
    public class ContatoController : Controller
    {
        private readonly ContatoDal _contatoDal;

        public ContatoController(ContatoDal contatoDal)
        {
            _contatoDal = contatoDal;
        }

        public IActionResult Index()
        {
            var contatos = _contatoDal.GetContatos();
            return View(contatos);
        }
    }

}
