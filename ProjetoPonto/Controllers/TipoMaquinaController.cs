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
    public class TipoMaquinaController : Controller
    {
        private TipoMaquinaModel tipoMaquinaModel = new TipoMaquinaModel();

        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador") || (System.Web.Security.Roles.IsUserInRole(User.Identity.Name, "gerencia")))
            {
                return View(tipoMaquinaModel.todosTipoMaquina());
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Index(string texto)
        {
            return View(tipoMaquinaModel.PesquisaTipoMaquinas(texto));
        }
        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador") || (System.Web.Security.Roles.IsUserInRole(User.Identity.Name, "gerencia")))
            {
                TipoMaquina t = new TipoMaquina();
                ViewBag.Titulo = "Nova TipoMaquina";
                if (id != 0)
                {
                    t = tipoMaquinaModel.obterTipoMaquina(id);
                    ViewBag.Titulo = "Editar TipoMaquina";
                }
                return View(t);
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Edit(TipoMaquina t)
        {
            string erro = null;
            if (t.IdTipoMaquina== 0)
                erro = tipoMaquinaModel.adicionarTipoMaquina(t);
            else
                erro = tipoMaquinaModel.editarTipoMaquina(t);
            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(t);
            }
        }
        public ActionResult Delete(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador") || (System.Web.Security.Roles.IsUserInRole(User.Identity.Name, "gerencia")))
            {
            TipoMaquina t = tipoMaquinaModel.obterTipoMaquina(id);
            tipoMaquinaModel.excluirTipoMaquina(t);
            return RedirectToAction("Index");
            }
            return Redirect("/Shared/Error");
        }
    }
}
