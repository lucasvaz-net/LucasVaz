using LucasVaz.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LucasVaz.Controllers
{
    public class EditarExperienciaController : Controller
    {
        private readonly ExperienciaDal _experienciaDal;

        public EditarExperienciaController(ExperienciaDal experienciaDal)
        {
            _experienciaDal = experienciaDal;
        }

        [Authorize]
        public IActionResult Index()
        {
            var experiencia = _experienciaDal.GetExperiencias();
            return View(experiencia);
        }
    }
}
