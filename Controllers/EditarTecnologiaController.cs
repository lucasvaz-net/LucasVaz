using LucasVaz.Data;
using LucasVaz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LucasVaz.Controllers
{
    [Authorize]
    public class EditarTecnologiaController : Controller
    {
        private readonly HabilidadeDal _habilidadeDal;

        public EditarTecnologiaController(HabilidadeDal habilidadeDal)
        {
            _habilidadeDal = habilidadeDal;
        }

        public IActionResult Index()
        {
            var habilidades = _habilidadeDal.GetHabilidades();

            if (!habilidades.Any())
            {
                return View("Error", new string("Nenhuma habilidade/tecnologia encontrada"));
            }

            return View(habilidades);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Tecnologia tecnologia)
        {
            if (ModelState.IsValid)
            {
                _habilidadeDal.UpsertTecnologia(tecnologia, "I"); 
                return RedirectToAction(nameof(Index));
            }
            return View(tecnologia);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var tecnologia = _habilidadeDal.GetHabilidadeById(id);
            if (tecnologia == null)
            {
                return NotFound();
            }
            return View(tecnologia);
        }

        [HttpPost]
        public IActionResult Edit(Tecnologia tecnologia)
        {
            if (ModelState.IsValid)
            {
                _habilidadeDal.UpsertTecnologia(tecnologia, "U");  
                return RedirectToAction(nameof(Index));
            }
            return View(tecnologia);
        }
    }
}
