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
    public class PontoController : Controller
    {
        private PontoModel pontoModel = new PontoModel();
        private UsuarioModel usuarioModel = new UsuarioModel();
        private OsModel osModel = new OsModel();
        private TipoPontoModel tipoPontoModel = new TipoPontoModel();

        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "mecanico"))
            {
                return View(pontoModel.todosPontos());
            }
            return Redirect("/Shared/Error");
        }


        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "mecanico"))
            {
                
                Ponto p = new Ponto();
                
                int idUsuario = 1;
                int idOs = 1;
                int idTipoPonto = 1;
                DateTime dataAbertura = DateTime.Now;
                TimeSpan horaInicial = DateTime.Now.TimeOfDay;
                
            
                if (id != 0)
                {
                    p = pontoModel.obterPonto(id);
                    idUsuario = p.IdUsuario;
                    idOs = p.IdOs;
                    idTipoPonto = p.IdTipoPonto;
                    dataAbertura = p.DataAbertura.Date;
                    horaInicial = p.HoraInicial;
                   
                }
                ViewBag.HoraInicial = horaInicial.ToString(@"hh\:mm\:ss"); 
               // ViewBag.HoraInicial = horaInicial.ToString(@"HH:mm:ss");
                ViewBag.DataAbertura = dataAbertura.ToString(@"dd/MM/yyyy");
                ViewBag.IdUsuario = new SelectList(usuarioModel.todosUsuarios(), "IdUsuario", "Login", idUsuario);
                ViewBag.IdOs = new SelectList(osModel.todasOs(), "IdOs", "NumeroOs", idOs);
                ViewBag.IdTipoPonto = new SelectList(tipoPontoModel.todosTipoPonto(), "IdTipoPonto", "Descricao", idTipoPonto);
                return View(p);
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Edit(Ponto p)
        {

            string erro = null;
            if (p.IdPonto == 0)
                erro = pontoModel.adicionarPonto(p);
            else
                erro = pontoModel.editarPonto(p);
            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(p);
            }
        }
        public ActionResult Delete(int id)
        {
            Ponto p = pontoModel.obterPonto(id);
            pontoModel.excluirPonto(p);
            return RedirectToAction("Index");
        }
    }
}
