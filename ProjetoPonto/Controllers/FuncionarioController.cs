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
    public class FuncionarioController : Controller
    {
        private FuncionarioModel funcionarioModel = new FuncionarioModel();
       
        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
            return View(funcionarioModel.todosFuncionarios());
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Index(string texto)
        {        
            return View(funcionarioModel.PesquisaFuncionarios(texto));
        }
        public ActionResult Edit(int id)
        {
             if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
            Funcionario f = new Funcionario();
            ViewBag.Titulo = "Nova Funcionario";
            if (id != 0)
            {
                f = funcionarioModel.obterFuncionario(id);
                ViewBag.Titulo = "Editar Funcionario";
            }
            return View(f);
        }
             return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Edit(Funcionario f)
        {
            string erro = funcionarioModel.validarFuncionario(f);
            if (f.IdFuncionario == 0)
                erro = funcionarioModel.adicionarFuncionario(f);
            else
                erro = funcionarioModel.editarFuncionario(f);
            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(f);
            }
        }
        public ActionResult Delete(int id)
        {
            Funcionario f = funcionarioModel.obterFuncionario(id);
            funcionarioModel.excluirFuncionario(f);
            return RedirectToAction("Index");
        }
    }
}
