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
    public class EstadoController : Controller
    {
        private EstadoModel estadoModel = new EstadoModel();

        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
            return View(estadoModel.todosEstados());
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Index(string texto)
        {
            return View(estadoModel.PesquisaEstados(texto));
        }
        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
            Estado e = new Estado();
            ViewBag.Titulo = "Novo Estado";
            if (id != 0)
            {
                e = estadoModel.obterEstado(id);
                ViewBag.Titulo = "Editar Estado";
            }
            return View(e);
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Edit(Estado e)
        {
            string erro = null;
            if (e.IdEstado == 0)
                erro = estadoModel.adicionarEstado(e);
            else
                erro = estadoModel.editarEstado(e);
            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(e);
            }
        }
        public ActionResult Delete(int id)
        {
            Estado e = estadoModel.obterEstado(id);
            estadoModel.excluirEstado(e);
            return RedirectToAction("Index");
        }
    }
}
