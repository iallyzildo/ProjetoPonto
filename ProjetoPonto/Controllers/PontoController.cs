﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjetoPonto.Entity;
using ProjetoPonto.Models;
using Rotativa;
using Rotativa.Options;
using System.Web.Security;
using PagedList;
using PagedList.Mvc;
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
                string login = User.Identity.Name;
                return View(pontoModel.todosPontosAbertosPorUsuario(login));
            }
            return Redirect("/Shared/Error");
        }

        public ActionResult ConsultaRegistros(int? pagina)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
                int tamanhoPagina = 15;
                int numeroPagina = pagina ?? 1;
                return View(pontoModel.todosPontosFechados().ToPagedList(numeroPagina, tamanhoPagina));
            }
            return Redirect("/Shared/Error");
        }

        public ActionResult BuscaRegistros()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
                return View(pontoModel.todosPontosFechados());
            }
            return Redirect("/Shared/Error");
        }

        [HttpPost]
        public ActionResult BuscaRegistros(string os)
        {     
                var pdf = new ViewAsPdf(pontoModel.PesquisaPontosOs(os))
             {
                CustomSwitches = "--print-media-type --header-center \"Redemaq Minas\""
                //Model = modelo
            };
                return pdf;
        }




        public ActionResult Registro(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "mecanico"))
            {
                
                Ponto p = new Ponto();
                string login = User.Identity.Name;
                Usuario u = usuarioModel.obterUsuarioPorLogin(login);

                int idUsuario = u.IdUsuario;
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
                    TimeSpan horaFinal = DateTime.Now.TimeOfDay;
                    ViewBag.HoraFinal = horaFinal.ToString(@"hh\:mm\:ss"); 
                   
                }
                ViewBag.HoraInicial = horaInicial.ToString(@"hh\:mm\:ss"); 
                ViewBag.DataAbertura = dataAbertura.ToString(@"dd/MM/yyyy");
                ViewBag.IdUsuario = idUsuario;
                ViewBag.IdOs = new SelectList(osModel.todasOs(), "IdOs", "NumeroOs", idOs);
                ViewBag.IdTipoPonto = new SelectList(tipoPontoModel.todosTipoPonto(), "IdTipoPonto", "Descricao", idTipoPonto);
                return View(p);
            }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Registro(Ponto p)
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
