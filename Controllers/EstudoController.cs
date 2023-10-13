using LucasVaz.Models;
using LucasVaz.Data;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace LucasVaz.Controllers
{
    public class EstudoController : Controller
    {
        private readonly EstudoDal _estudoDal;
        private const int PageSize = 6;
        public EstudoController(EstudoDal estudoDal)
        {
            _estudoDal = estudoDal;
        }

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
                return View();  
            }
        }
    }
}
