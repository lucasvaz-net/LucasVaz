using LucasVaz.Data;
using LucasVaz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LucasVaz.Controllers
{
    public class EditarEstudoController : Controller
    {
        private readonly EstudoDal _estudoDal;

        public EditarEstudoController(EstudoDal estudoDal)
        {
            _estudoDal = estudoDal;
        }

        [Authorize]
        public IActionResult Index()
        {
            var estudo = _estudoDal.GetEstudos();
            return View(estudo);
        }
    }
}

