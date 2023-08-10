using LucasVaz.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LucasVaz.Controllers
{
    [Authorize]
    public class EditarContatoController : Controller
    {
        private readonly ContatoDal _contatoDal;

        public EditarContatoController(ContatoDal contatoDal)
        {
            _contatoDal = contatoDal;
        }

        public IActionResult Index()
        {
            var contatos = _contatoDal.GetContatos();

            if (contatos == null || !contatos.Any())
            {
                // Aqui, você pode optar por retornar uma view de erro ou uma mensagem para o usuário.
                return View("Error", new string("Nenhum contato encontrado"));
            }

            return View(contatos);
        }
    }
}
