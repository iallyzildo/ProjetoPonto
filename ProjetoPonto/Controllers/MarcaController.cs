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
    public class MarcaController : Controller
    {
        private MarcaModel marcaModel = new MarcaModel();

        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "admin"))
            {
                return View(marcaModel.todasMarcas());
            }
            return Redirect("/Shared/Error");
        }
        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "admin"))
            {
                Marca m = new Marca();
                ViewBag.Titulo = "Nova Marca";
                if (id != 0)
                {
                    m = marcaModel.obterMarca(id);
                    ViewBag.Titulo = "Editar Marca";
                }
                return View(m);
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Edit(Marca m)
        {
            string erro = null;
            if (m.IdMarca == 0)
                erro = marcaModel.adicionarMarca(m);
            else
                erro = marcaModel.editarMarca(m);
            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(m);
            }
        }
        public ActionResult Delete(int id)
        {
            Marca m = marcaModel.obterMarca(id);
            marcaModel.excluirMarca(m);
            return RedirectToAction("Index");
        }
    }
}
