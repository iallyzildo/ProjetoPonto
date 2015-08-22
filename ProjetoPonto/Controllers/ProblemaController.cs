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
    public class ProblemaController : Controller
    {
        private ProblemaModel problemaModel = new ProblemaModel();
        private OsModel osModel = new OsModel();
        private SecaoProblemaModel secaoProblemaModel = new SecaoProblemaModel();

        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
                return View(problemaModel.todosProblemas());
            }
            return Redirect("/Shared/Error");
        }
        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
                Problema p = new Problema();
                int idOs = 1;
                int idSecao = 1;

                if (id != 0)
                {
                    p = problemaModel.obterProblema(id);
                    idOs= p.IdOs;
                    idSecao = p.IdSecao;
                }

                ViewBag.IdOs= new SelectList(osModel.todasOs(), "IdOs", "NumeroOs", idOs);
                ViewBag.IdSecao = new SelectList(secaoProblemaModel.todasSecaoProblema(), "IdSecao", "Descricao", idSecao);

                return View(p);
            }
            return Redirect("/Shared/Error");
        }

        [HttpPost]
        public ActionResult Edit(Problema p)
        {
            string erro = problemaModel.validarProblema(p);
            if (erro == null)
            {
                if (p.IdProblema == 0)
                {
                    erro = problemaModel.adicionarProblema(p);
                }
                else
                {
                    erro = problemaModel.editarProblema(p);
                }
            }

            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(p);
            }
        }
        public ActionResult Delete(int id)
        {
            Problema p = problemaModel.obterProblema(id);
            problemaModel.excluirProblema(p);
            return RedirectToAction("Index");
        }
    }
}
