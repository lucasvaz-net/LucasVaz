using LucasVaz.Data;
using LucasVaz.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LucasVaz.Controllers
{
    public class EditarProjetoController : Controller
    {
        private readonly ProjetoDal _projetoDal;

        public EditarProjetoController(ProjetoDal projetoDal)
        {
            _projetoDal = projetoDal;
        }

        [Authorize]
        public IActionResult Index(int pageNumber = 1, int pageSize = 6)
        {
            var projetos = _projetoDal.GetAllProjetos(pageNumber, pageSize);
            return View(projetos);
        }

        [Authorize]
        [HttpGet]
        public IActionResult InserirProjeto()
        {
            return View(new Projeto());  
        }

        [Authorize]
        [HttpPost]
        public IActionResult InserirProjeto(Projeto projeto)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    _projetoDal.InserirProjeto(projeto);

                    TempData["Message"] = "Projeto inserido com sucesso!";
                    return RedirectToAction("Index");
                }

                TempData["Message"] = "Verifique os dados e tente novamente.";
                return View(projeto);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Erro detalhado: " + ex.Message + " - " + ex.StackTrace;
                return View(projeto);
            }

        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            Projeto projeto = _projetoDal.GetProjetoById(id);

            if (projeto == null)
            {
                return NotFound();
            }

            return View(projeto);
        }

        [HttpPost]
        public IActionResult Edit(Projeto projeto)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    
                    _projetoDal.EditarProjeto(projeto);

                    TempData["SuccessMessage"] = "Projeto atualizado com sucesso!";
                    return RedirectToAction("Index");
                }
                else
                {
                    
                    var errorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList();
                    TempData["ErrorMessage"] = string.Join(" | ", errorList);
                }
            }
            catch (Exception ex)
            {
                
                TempData["ErrorMessage"] = $"Erro ao atualizar projeto: {ex.Message}";
            }

            
            return View(projeto);
        }


        [HttpGet]
        public IActionResult GerenciarTecnologias(int idProjeto)
        {
            
            ViewBag.IdProjeto = idProjeto;
            var tecnologias = _projetoDal.ObterTodasAsTecnologias();

            return View(tecnologias);
        }


        [HttpPost]
        public IActionResult GerenciarTecnologias(int idProjeto, int idTecnologia, char operacao)
        {
            try
            {
                _projetoDal.GerenciarTecnologiaProjeto(idProjeto, idTecnologia, operacao);
                return RedirectToAction("Edit", new { id = idProjeto });
            }
            catch (Exception ex)
            {
                ViewBag.IdProjeto = idProjeto;
                var tecnologias = _projetoDal.ObterTodasAsTecnologias();
                ModelState.AddModelError("", "Ocorreu um erro ao gerenciar tecnologias do projeto: " + ex.Message);
                return View(tecnologias);
            }

        }



    }
}
