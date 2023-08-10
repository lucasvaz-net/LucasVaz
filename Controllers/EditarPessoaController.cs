using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LucasVaz.Data;
using LucasVaz.Models;

namespace LucasVaz.Controllers
{
    [Authorize]
    public class EditarPessoaController : Controller
    {
        private readonly PessoaDal _pessoaDal;

        public EditarPessoaController(PessoaDal pessoaDal)
        {
            _pessoaDal = pessoaDal;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var pessoa = _pessoaDal.GetPessoa(1); 
            return View(pessoa);
        }

        [HttpPost]
        public IActionResult Index(Pessoa pessoa)
        {
            if (ModelState.IsValid)
            {
                _pessoaDal.EditarPessoa(pessoa);
                TempData["SuccessMessage"] = "Dados atualizados com sucesso!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Erro ao atualizar os dados. Por favor, verifique os campos.";
                return View(pessoa);
            }
        }
    }
}
