using LucasVaz.Data;
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
                // Aqui, você pode optar por retornar uma view de erro ou uma mensagem para o usuário.
                return View("Error", new string("Nenhum registro de privacidade encontrado"));
            }

            return View(privacidades);
        }
    }
}
