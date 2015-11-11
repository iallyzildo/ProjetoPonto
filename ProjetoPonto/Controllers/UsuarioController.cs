using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using ProjetoPonto.Entity;
using ProjetoPonto.Models;
using System.Web.Helpers;


namespace ProjetoPonto.Controllers
{
    
    public class UsuarioController : Controller
    {
        private UsuarioModel usuarioModel = new UsuarioModel();
        private OsModel osModel = new OsModel();
        private PontoModel pontoModel = new PontoModel();
        private FuncionarioModel funcionarioModel = new FuncionarioModel();
        private EmpresaModel empresaModel = new EmpresaModel();
        private PerfilModel perfilModel = new PerfilModel();


        public UsuarioController()
        {
            WebMail.SmtpServer ="smtp.gmail.com";
            WebMail.EnableSsl =true;
            WebMail.SmtpPort =587;
            WebMail.From = "iallyleandro1994@gmail.com";
            WebMail.UserName = "iallyleandro1994@gmail.com";
            WebMail.Password ="SENHA AQUI";
        }

        [Authorize]
        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
            return View(usuarioModel.todosUsuarios());
            }
             return Redirect("/Shared/Error");
        }
        [Authorize]
        public ActionResult Email()
        {

                return View();

        }

        [HttpPost]
        public ActionResult Index(string texto)
        {
            return View(usuarioModel.PesquisaUsuarios(texto));
        }

         [Authorize]
        public ActionResult AreaRestrita()
        {
            Usuario u = new Usuario();
            int totalUsuarios = usuarioModel.qtdUsuarios();
            int totalUsuariosAdministradores = usuarioModel.qtdUsuariosAdministradores();
            int totalUsuariosMecanicos = usuarioModel.qtdUsuariosMecanicos();
            int totalUsuariosGerentes = usuarioModel.qtdUsuariosGerentes();

            Ponto p = new Ponto();
            int totalPontos = pontoModel.qtdPontos();
            int totalPontosAbertos = pontoModel.qtdPontosAbertos();

            Os o = new Os();
            int totalOs = osModel.qtdOs();
          


            ViewBag.TotalUsuarios = totalUsuarios;
            ViewBag.TotalPontos   = totalPontos;
            ViewBag.TotalOs = totalOs;
            ViewBag.TotalPontosAbertos = totalPontosAbertos;
            ViewBag.TotalUsuariosAdministradores = totalUsuariosAdministradores;
            ViewBag.TotalUsuariosMecanicos = totalUsuariosMecanicos;
            ViewBag.TotalUsuariosGerentes = totalUsuariosGerentes;


            if (Roles.IsUserInRole(User.Identity.Name, "administrador")||(System.Web.Security.Roles.IsUserInRole(User.Identity.Name, "gerencia")))
            {
                return View("AreaRestrita", usuarioModel.todosUsuarios());
               
            }
            return Redirect("/Shared/Error");     

        }

    

       // POST : / Email / Envia
       [HttpPost]
       public ActionResult Envia(string mensagem)
       {
           WebMail.Send("ti.redemaqminas@gmail.com", "Mensagem do sistema", usuarioModel.EnviarEmail(mensagem));
           return View();

       }

        public ActionResult Login()
        {

            return View(new Usuario());

        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
            Usuario u = new Usuario();
            u = usuarioModel.obterUsuario(id);
              
            int idFuncionario = 1;
            int idEmpresa = 1;
            int idPerfil = 1;
            if (id != 0)
            {
                u = usuarioModel.obterUsuario(id);
                idFuncionario = u.IdFuncionario;
                idEmpresa = u.IdEmpresa;
                idPerfil = u.IdPerfil;
            }

            ViewBag.IdFuncionario = new SelectList(funcionarioModel.todosFuncionarios(), "IdFuncionario", "Nome", idFuncionario);
            ViewBag.IdEmpresa = new SelectList(empresaModel.todasEmpresas(), "IdEmpresa", "RazaoSocial", idEmpresa);
            ViewBag.IdPerfil= new SelectList(perfilModel.todosPerfis(), "IdPerfil", "Descricao", idPerfil);

            return View(u);
        }
            return Redirect("/Shared/Error");
        }
        [HttpPost]
        public ActionResult Edit(Usuario u)
        {
            string erro = usuarioModel.validarUsuario(u);
            if (erro == null)
            {
                if (u.IdUsuario == 0)
                {
                    erro = usuarioModel.adicionarUsuario(u);
                }
                else
                {
                    erro = usuarioModel.editarUsuario(u);
                }
            }

            if (erro == null)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Erro = erro;
                return View(u);
            }
        }
        [HttpPost]
        public ActionResult Login(Usuario u)
        {
            Usuario banco = usuarioModel.obterUsuarioPorLogin(u.Login);
            if (banco == null || banco == new Usuario())
            {
                ViewBag.Erro = "Usuário inexistente!";
                return View(u);
            }
            if (u.Senha != banco.Senha)
            {
                ViewBag.Erro = "Senha Incorreta!";
                return View(u);
            }

            Roles.DeleteCookie();
            // Passar perfis do banco para aplicação
            foreach (Perfil p in perfilModel.todosPerfis())
            {
                if (!Roles.RoleExists(p.Descricao))// Testa se a role nao existe
                {
                    Roles.CreateRole(p.Descricao);// adiciona a role
                }
            }

            //Adicionar perfis do usuarui a classe Role
            foreach (Perfil p in perfilModel.listarPerfisPorUsuario(banco.IdUsuario))
            {
                // Testa se o mecanico nao está na role associada ao banco
                if (!Roles.IsUserInRole(u.Login, p.Descricao))
                {
                    Roles.AddUserToRole(u.Login, p.Descricao); // adiciona o usuario
                }
            }

            FormsAuthentication.SetAuthCookie(u.Login, true);

            return Redirect("/");
                        
        }

        public ActionResult Logoff()
        {
            //LIMPAR TODO CACHE
            //string currentKey = "";
            //System.Collections.IDictionaryEnumerator cacheContents = System.Web.HttpContext.Current.Cache.GetEnumerator();
            //while (cacheContents.MoveNext())
            //{
            //    currentKey = cacheContents.Key.ToString();
            //    System.Web.HttpContext.Current.Cache.Remove(currentKey);
            //}
            Usuario u = usuarioModel.obterUsuarioPorLogin(User.Identity.Name);
            // Remover todos os perfis do usuario
            foreach (Perfil p in perfilModel.listarPerfisPorUsuario(u.IdUsuario))
            {
                if (Roles.IsUserInRole(u.Login, p.Descricao))
                {
                    Roles.RemoveUserFromRole(u.Login, p.Descricao); // adiciona o usuario
             
                }
            }
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
        public ActionResult Delete(int id)
        {

            Usuario u = usuarioModel.obterUsuario(id);

            if (u.Login == User.Identity.Name)
            {
                return Redirect("/Shared/Error");
            }
            usuarioModel.excluirUsuario(u);
            return RedirectToAction("Index");
        }

         
              

    }
}
