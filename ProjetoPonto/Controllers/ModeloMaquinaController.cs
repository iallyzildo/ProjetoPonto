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
    public class ModeloMaquinaController : Controller
    {
        private MarcaModel marcaModel = new MarcaModel();
        private TipoMaquinaModel tipoMaquinaModel = new TipoMaquinaModel();
        private ModeloMaquinaModel modeloMaquinaModel = new ModeloMaquinaModel();

        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador") || (System.Web.Security.Roles.IsUserInRole(User.Identity.Name, "gerencia")))
            {
                return View(modeloMaquinaModel.todosModeloMaquina());
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Index(string texto)
        {
            return View(modeloMaquinaModel.PesquisaModeloMaquinas(texto));
        }
        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador") || (System.Web.Security.Roles.IsUserInRole(User.Identity.Name, "gerencia")))
            {
                ModeloMaquina m = new ModeloMaquina();
                int idMarca = 1;
                int idTipoMaquina = 1;
                if (id != 0)
                {
                    m = modeloMaquinaModel.obterModeloMaquina(id);
                    idMarca = m.IdMarca;
                    idTipoMaquina= m.IdTipoMaquina;
                }

                ViewBag.IdMarca = new SelectList(marcaModel.todasMarcas(), "IdMarca", "Descricao", idMarca);
                ViewBag.IdTipoMaquina = new SelectList(tipoMaquinaModel.todosTipoMaquina(), "IdTipoMaquina", "Descricao", idTipoMaquina);



                return View(m);
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Edit(ModeloMaquina m)
        {
            string erro = modeloMaquinaModel.validarModeloMaquina(m);
            if (erro == null)
            {
                if (m.IdModeloMaquina == 0)
                {
                    erro = modeloMaquinaModel.adicionarModeloMaquina(m);
                }
                else
                {
                    erro = modeloMaquinaModel.editarModeloMaquina(m);
                }
            }

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
             if (Roles.IsUserInRole(User.Identity.Name, "administrador") || (System.Web.Security.Roles.IsUserInRole(User.Identity.Name, "gerencia")))
            {
            ModeloMaquina m = modeloMaquinaModel.obterModeloMaquina(id);
            modeloMaquinaModel.excluirModeloMaquina(m);
            return RedirectToAction("Index");
            }
             return Redirect("/Shared/Error");
        }

    }
}
