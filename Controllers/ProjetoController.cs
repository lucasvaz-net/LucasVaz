using Microsoft.AspNetCore.Mvc;

namespace LucasVaz.Controllers
{
    public class ProjetoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
