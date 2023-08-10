using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LucasVaz.Controllers
{
    public class EditarProjetoController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
