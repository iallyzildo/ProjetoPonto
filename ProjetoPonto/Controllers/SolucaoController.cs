using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetoPonto.Entity;
using ProjetoPonto.Models;
using System.Web.Security;

namespace ProjetoPonto.Controllers
{
    [Authorize]
    public class SolucaoController : Controller
    {
        private SolucaoModel solucaoModel = new SolucaoModel();
        private ProblemaModel problemaModel = new ProblemaModel();


        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador") || (System.Web.Security.Roles.IsUserInRole(User.Identity.Name, "gerencia")))
            {
                return View(solucaoModel.todasSolucoes());
            }
            return Redirect("/Shared/Error");
        }
        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador") || (System.Web.Security.Roles.IsUserInRole(User.Identity.Name, "gerencia")))
            {
                Solucao s = new Solucao();
                int idProblema = 1;


                if (id != 0)
                {
                    s = solucaoModel.obterSolucao(id);
                    idProblema = s.IdProblema;

                }

                ViewBag.IdProblema = new SelectList(problemaModel.todosProblemas(), "IdProblema", "Descricao", idProblema);

                return View(s);
            }
            return Redirect("/Shared/Error");
        }

        [HttpPost]
        public ActionResult Edit(Solucao s)
        {
            string erro = solucaoModel.validarSolucao(s);
            if (erro == null)
            {
                if (s.IdSolucao == 0)
                {
                    erro = solucaoModel.adicionarSolucao(s);
                }
                else
                {
                    erro = solucaoModel.editarSolucao(s);
                }
            }

            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(s);
            }
        }
        public ActionResult Delete(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador") || (System.Web.Security.Roles.IsUserInRole(User.Identity.Name, "gerencia")))
            {
            Solucao s = solucaoModel.obterSolucao(id);
            solucaoModel.excluirSolucao(s);
            return RedirectToAction("Index");
        }
            return Redirect("/Shared/Error");
        }
    }
}
