using LucasVaz.Data;
using LucasVaz.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LucasVaz.Controllers
{
    public class ProjetoController : Controller
    {
        private readonly ProjetoDal _projetoDal;

        public ProjetoController(ProjetoDal projetoDal)
        {
            _projetoDal = projetoDal;
        }

        [HttpPost]
        public IActionResult FiltrarPorTecnologia(List<int> idsTecnologia, int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int size = pageSize ?? 6;
            var projetos = _projetoDal.GetProjetosPorTecnologia(idsTecnologia, pageNumber, size);
            ViewBag.Tecnologias = _projetoDal.ObterTodasAsTecnologias();
            return View("Index", projetos);
        }

        public IActionResult Index(int? page, int? pageSize)
        {
            int pageNumber = page ?? 1;
            int size = pageSize ?? 6;
            var projetos = _projetoDal.GetProjetos(pageNumber, size);
            ViewBag.Tecnologias = _projetoDal.ObterTodasAsTecnologias();
            return View(projetos);
        }

        public IActionResult Detalhes(int id)
        {
            Projeto projeto = _projetoDal.GetProjetoById(id);

            if (projeto == null)
            {
                return NotFound();
            }

            return View(projeto);
        }

    }
}
