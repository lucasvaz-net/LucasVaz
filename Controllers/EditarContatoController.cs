using LucasVaz.Data;
using LucasVaz.Models;
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


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var contato = _contatoDal.GetContatoById(id);
            if (contato == null)
            {
                return NotFound();
            }
            return View(contato);
        }

        [HttpPost]
        public IActionResult Edit(int id, Contato contato)
        {
            if (id != contato.IdContato)
            {
                return BadRequest();
            }

            try
            {
                _contatoDal.UpdateContato(contato); 
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View("Error", new string("Erro ao atualizar o contato"));
            }
        }


    }
}
