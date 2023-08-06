using LucasVaz.Data;
using Microsoft.AspNetCore.Mvc;

namespace LucasVaz.Controllers
{
    public class PrivacidadeController : Controller
    {
        private readonly PrivacidadeDal _privacidadeDal;

        public PrivacidadeController(PrivacidadeDal privacidadeDal)
        {
            _privacidadeDal = privacidadeDal;
        }

        public IActionResult Index()
        {
            var privacidades = _privacidadeDal.GetPrivacidades();
            return View(privacidades);
        }
    }
}
