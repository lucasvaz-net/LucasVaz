using LucasVaz.Models;
using LucasVaz.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LucasVaz.Controllers
{
    public class ExperienciaController : Controller
    {
        private readonly ExperienciaDal _experienciaDal;

        public ExperienciaController(ExperienciaDal experienciaDal)
        {
            _experienciaDal = experienciaDal;
        }

        public IActionResult Index()
        {
            var experiencia = _experienciaDal.GetExperiencias();
            return View(experiencia);
        }
    }
}
