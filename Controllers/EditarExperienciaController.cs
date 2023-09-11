using LucasVaz.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LucasVaz.Controllers
{
    [Authorize]
    public class EditarExperienciaController : Controller
    {
        private readonly ExperienciaDal _experienciaDal;
        private const int DefaultPageSize = 10;

        public EditarExperienciaController(ExperienciaDal experienciaDal)
        {
            _experienciaDal = experienciaDal ?? throw new ArgumentNullException(nameof(experienciaDal), "ExperienciaDal cannot be null.");
        }

        public IActionResult Index(int pageNumber = 1, int pageSize = DefaultPageSize)
        {
            try
            {
                var experiencias = _experienciaDal.GetExperiencias(pageNumber, pageSize);
                return View(experiencias);
            }
            catch (Exception ex)
            {

                ViewBag.ErrorMessage = "Failed to load experiencias for editing. Please try again later.";
                return View(); 
            }
        }
    }
}
