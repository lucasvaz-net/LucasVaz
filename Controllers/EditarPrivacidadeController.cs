using LucasVaz.Data;
using LucasVaz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LucasVaz.Controllers
{
    [Authorize]
    public class EditarPrivacidadeController : Controller
    {
        private readonly PrivacidadeDal _privacidadeDal;

        public EditarPrivacidadeController(PrivacidadeDal privacidadeDal)
        {
            _privacidadeDal = privacidadeDal;
        }

        public IActionResult Index()
        {
            var privacidades = _privacidadeDal.GetPrivacidades();

            if (privacidades == null || !privacidades.Any())
            {
                
                return View("Error", new string("Nenhum registro de privacidade encontrado"));
            }

            return View(privacidades);
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var privacidade = _privacidadeDal.GetPrivacidadeById(id);

            if (privacidade == null)
            {
                return NotFound();
            }

            return View(privacidade);
        }


        [HttpPost]
        public IActionResult Edit(Privacidade privacidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool updated = _privacidadeDal.UpsertPrivacidade(privacidade, "U");

                    if (updated)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao atualizar os dados.");
                }
                catch (Exception ex)
                {
                    
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao atualizar os dados.");
                }
            }

            return View(privacidade);  
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View(new Privacidade()); 
        }

        [HttpPost]
        public IActionResult Create(Privacidade privacidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool inserted = _privacidadeDal.UpsertPrivacidade(privacidade, "I");

                    if (inserted)
                        return RedirectToAction("Index");
                    else
                        ModelState.AddModelError(string.Empty, "Ocorreu um erro ao inserir os dados.");
                }
                catch (Exception ex)
                {
                   
                    ModelState.AddModelError(string.Empty, "Ocorreu um erro ao inserir os dados.");
                }
            }

            return View(privacidade); 
        }


    }
}
