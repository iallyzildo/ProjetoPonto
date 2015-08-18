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
    public class OsController : Controller
    {
        private ClienteModel clienteModel = new ClienteModel();
        private ModeloMaquinaModel modeloMaquinaModel = new ModeloMaquinaModel();
        private StatusOsModel statusOsModel = new StatusOsModel();
        private OsModel osModel = new OsModel();

        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
                return View(osModel.todasOs());
            }
            return Redirect("/Shared/Error");
        }
        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
                Os o = new Os();
                int idStatusOs = 1;
                int idModeloMaquina = 1;
                int idCliente = 1;
                if (id != 0)
                {
                    o = osModel.obterOs(id);
                    idStatusOs = o.IdStatusOs;
                    idModeloMaquina = o.IdModeloMaquina;
                    idCliente = o.IdCliente;

                }

                ViewBag.IdStatusOs= new SelectList(statusOsModel.todosStatusOs(), "IdStatusOs", "Descricao", idStatusOs);
                ViewBag.IdModeloMaquina = new SelectList(modeloMaquinaModel.todosModeloMaquina(), "IdModeloMaquina", "Descricao", idModeloMaquina);
                ViewBag.IdCliente = new SelectList(clienteModel.todosClientes(), "IdCliente", "Nome", idCliente);


                return View(o);
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Edit(Os o)
        {
            string erro = osModel.validarOs(o);
            if (erro == null)
            {
                if (o.IdOs == 0)
                {
                    erro = osModel.adicionarOs(o);
                }
                else
                {
                    erro = osModel.editarOs(o);
                }
            }

            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(o);
            }
        }
        public ActionResult Delete(int id)
        {
            Os o = osModel.obterOs(id);
            osModel.excluirOs(o);
            return RedirectToAction("Index");
        }

    }
}
