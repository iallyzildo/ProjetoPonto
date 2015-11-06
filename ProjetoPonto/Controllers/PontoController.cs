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
using System.Web.Helpers;

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

        public PontoController()
        {
            WebMail.SmtpServer ="smtp.gmail.com";
            WebMail.EnableSsl =true;
            WebMail.SmtpPort =587;
            WebMail.From = "pontoredemaq@gmail.com";
            WebMail.UserName = "pontoredemaq@gmail.com";
            WebMail.Password ="x1x2x3x4";
        }

        public ActionResult EnviaEmail(string mensagem)
        {
            WebMail.Send("iallyleandro1994@gmail.com", "Mensagem do sistema", usuarioModel.EnviarEmail(mensagem));
            return View();

        }


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
                ViewName = "Modelo",
                CustomSwitches = "--print-media-type --header-center \"Redemaq Minas\""

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



                    string dataInicio = p.DataAbertura.ToString(@"dd/MM/yyyy");
                    string dataAtual = DateTime.Now.ToString(@"dd/MM/yyyy");
                    string horaAtual = DateTime.Now.TimeOfDay.ToString(@"hh\:mm\:ss");


                    string mensagemEmail = "<h2> O usuario :" + p.Usuario.Login + " esta tentando finalizar o ponto :" + p.Os.NumeroOs + " no dia :" + dataAtual + " as :" + horaAtual + " que foi aberto dia :" + dataInicio + " as :" + p.HoraInicial + "<br/> | Favor confirmar com a gerencia a hora final para adicionar manualmente nos seus registros | </h2>";

                    if (dataInicio != dataAtual)
                    {
                        EnviaEmail(mensagemEmail);
                        return View("Aviso");
                    }
       
                   
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
