using LucasVaz.Data;
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

            if (habilidades == null || !habilidades.Any())
            {
                // Aqui, você pode optar por retornar uma view de erro ou uma mensagem para o usuário.
                return View("Error", new string("Nenhuma habilidade/tecnologia encontrada"));
            }

            return View(habilidades);
        }
    }
}
