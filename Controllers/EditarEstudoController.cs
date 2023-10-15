using LucasVaz.Data;
using LucasVaz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;
using System;

namespace LucasVaz.Controllers
{
    [Authorize]
    public class EditarEstudoController : Controller
    {
        private readonly EstudoDal _estudoDal;
        private const int PageSize = 6;

        public EditarEstudoController(EstudoDal estudoDal)
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

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Estudo());
        }

        [HttpPost]
        public IActionResult Create(Estudo estudo)
        {
            if (ModelState.IsValid)
            {
                _estudoDal.UpsertEstudo(estudo, "I");
                return RedirectToAction(nameof(Index));
            }

            return View(estudo);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var estudo = _estudoDal.GetEstudoById(id);

            if (estudo == null)
            {
                return NotFound();
            }

            return View(estudo);
        }

        [HttpPost]
        public IActionResult Edit(int id, Estudo estudo)
        {
            if (id != estudo.IdEstudo)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _estudoDal.UpsertEstudo(estudo, "U");
                return RedirectToAction(nameof(Index));
            }

            return View(estudo);
        }
    }
}
