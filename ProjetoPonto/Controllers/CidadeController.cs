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
    public class CidadeController : Controller
    {
        private CidadeModel cidadeModel = new CidadeModel();
        private EstadoModel estadoModel = new EstadoModel();

        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
            return View(cidadeModel.todasCidades());
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Index(string texto)
        {
            return View(cidadeModel.PesquisaCidades(texto));
        }
        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
            Cidade c = new Cidade();
            int idEstado = 1;
            if (id != 0)
            {
                c = cidadeModel.obterCidade(id);
                idEstado = c.IdEstado;
            }

            ViewBag.IdEstado = new SelectList(estadoModel.todosEstados(), "IdEstado", "Descricao", idEstado);

            return View(c);
                }
            return Redirect("/Shared/Error");
            }

        [HttpPost]
        public ActionResult Edit(Cidade c)
        {
            string erro = cidadeModel.validarCidade(c);
            if (erro == null)
            {
                if (c.IdCidade == 0)
                {
                    erro = cidadeModel.adicionarCidade(c);
                }
                else
                {
                    erro = cidadeModel.editarCidade(c);
                }
            }

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
            Cidade c = cidadeModel.obterCidade(id);
            cidadeModel.excluirCidade(c);
            return RedirectToAction("Index");
        }
    }
}
