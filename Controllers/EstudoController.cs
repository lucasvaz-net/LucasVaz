using Microsoft.AspNetCore.Mvc;

namespace LucasVaz.Controllers
{
    public class EstudoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
