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
    public class ClienteController : Controller
    {
        private ClienteModel clienteModel = new ClienteModel();

        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
                return View(clienteModel.todosClientes());
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Index(string texto)
        {
            return View(clienteModel.PesquisaClientes(texto));
        }
        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
                Cliente c = new Cliente();
                ViewBag.Titulo = "Novo Cliente";
                if (id != 0)
                {
                    c = clienteModel.obterCliente(id);
                    ViewBag.Titulo = "Editar Cliente";
                }
                return View(c);
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Edit(Cliente c)
        {
            string erro = null;
            if (c.IdCliente == 0)
                erro = clienteModel.adicionarCliente(c);
            else
                erro = clienteModel.editarCliente(c);
            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(c);
            }
        }
        public ActionResult Delete(int id)
        {
            Cliente c = clienteModel.obterCliente(id);
            clienteModel.excluirCliente(c);
            return RedirectToAction("Index");
        }
    }
}
