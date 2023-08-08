using LucasVaz.Data;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;


namespace LucasVaz.Controllers
{
    public class HabilidadeController : Controller
    {
        private readonly HabilidadeDal _habilidadeDal;

        public HabilidadeController(HabilidadeDal habilidadeDal)
        {
            _habilidadeDal = habilidadeDal;
        }

        public IActionResult Index()
        {
            var habilidades = _habilidadeDal.GetHabilidades();
            ViewBag.HabilidadesJson = JsonConvert.SerializeObject(habilidades);
            return View(habilidades);
        }
    }
}
