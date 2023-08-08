using LucasVaz.Models;
using LucasVaz.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LucasVaz.Controllers
{
    public class EstudoController : Controller
    {
        private readonly EstudoDal _estudoDal;

        public EstudoController(EstudoDal estudoDal)
        {
            _estudoDal = estudoDal;
        }

        public IActionResult Index()
        {
            var estudos = _estudoDal.GetEstudos();
            return View(estudos);
        }
    }
}
