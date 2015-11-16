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
    //[Authorize(Roles="Administrador")]
    public class SecaoProblemaController : Controller
    {
        private SecaoProblemaModel secaoProblemaModel = new SecaoProblemaModel();

        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador") || (System.Web.Security.Roles.IsUserInRole(User.Identity.Name, "gerencia")))
            {
                return View(secaoProblemaModel.todasSecaoProblema());
            }
            return Redirect("/Shared/Error");
        }
        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador") || (System.Web.Security.Roles.IsUserInRole(User.Identity.Name, "gerencia")))
            {
                SecaoProblema s = new SecaoProblema();
                ViewBag.Titulo = "Novo SecaoProblema";
                if (id != 0)
                {
                    s = secaoProblemaModel.obterSecaoProblema(id);
                    ViewBag.Titulo = "Editar SecaoProblema";
                }
                return View(s);
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Edit(SecaoProblema s)
        {
            string erro = null;
            if (s.IdSecao == 0)
                erro = secaoProblemaModel.adicionarSecaoProblema(s);
            else
                erro = secaoProblemaModel.editarSecaoProblema(s);
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
            SecaoProblema s = secaoProblemaModel.obterSecaoProblema(id);
            secaoProblemaModel.excluirSecaoProblema(s);
            return RedirectToAction("Index");
            }
             return Redirect("/Shared/Error");
        }
    }
}
