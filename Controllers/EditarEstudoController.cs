using LucasVaz.Data;
using LucasVaz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace LucasVaz.Controllers
{
    public class EditarEstudoController : Controller
    {
        private readonly EstudoDal _estudoDal;
        private const int PageSize = 10;  // Ajuste conforme sua necessidade

        public EditarEstudoController(EstudoDal estudoDal)
        {
            _estudoDal = estudoDal;
        }

        [Authorize]
        public IActionResult Index(int page = 1)
        {
            try
            {
                var estudos = _estudoDal.GetEstudos(page, PageSize);
                return View(estudos);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Houve um erro ao obter os estudos: {ex.Message}";
                return View();  // Você pode retornar para uma view de Erro, se preferir
            }
        }
    }
}
