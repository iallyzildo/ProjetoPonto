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
    public class EmpresaController : Controller
    {
        private CidadeModel cidadeModel = new CidadeModel();
        private EstadoModel estadoModel = new EstadoModel();
        private EmpresaModel empresaModel = new EmpresaModel();

        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
                return View(empresaModel.todasEmpresas());
            }
             return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Index(string texto)
        {
            return View(empresaModel.PesquisaEmpresas(texto));
        }
        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
            Empresa e = new Empresa();
            int idEstado = 1;
            int idCidade = 1;
            if (id != 0)
            {
                e = empresaModel.obterEmpresa(id);
                idEstado = e.Cidade.IdEstado;
                idCidade = e.IdCidade;
            }

            ViewBag.IdCidade = new SelectList(cidadeModel.listarCidadePorEstado(idEstado), "IdCidade", "Descricao", idCidade);
            ViewBag.IdEstado = new SelectList(estadoModel.todosEstados(), "IdEstado", "Descricao", idEstado);
            
            

            return View(e);
        }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Edit(Empresa e)
        {
            string erro = empresaModel.validarEmpresa(e);
            if (erro == null)
            {
                if (e.IdEmpresa == 0)
                {
                    erro = empresaModel.adicionarEmpresa(e);
                }
                else
                {
                    erro = empresaModel.editarEmpresa(e);
                }
            }

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
            Empresa e = empresaModel.obterEmpresa(id);
            empresaModel.excluirEmpresa(e);
            return RedirectToAction("Index");
        }

        public JsonResult ListaCidades(int idEstado)
        {
            var lista = new SelectList(cidadeModel.listarCidadePorEstado(idEstado), "IdCidade", "Descricao", 0);
            return Json(new { cidades = lista });
        }
    }
}
