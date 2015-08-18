using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.Mvc;
using ProjetoPonto.Entity;
using ProjetoPonto.Models;


namespace ProjetoPonto.Controllers
{
    
    public class UsuarioController : Controller
    {
        private UsuarioModel usuarioModel = new UsuarioModel();
        private FuncionarioModel funcionarioModel = new FuncionarioModel();
        private EmpresaModel empresaModel = new EmpresaModel();
        private PerfilModel perfilModel = new PerfilModel();
      
      
        
        [Authorize]
        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "administrador"))
            {
            return View(usuarioModel.todosUsuarios());
            }
             return Redirect("/Shared/Error");
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
                    Roles.AddUserToRole(u.Login, p.Descricao); // adiciona o mecanico
                }
            }

            FormsAuthentication.SetAuthCookie(u.Login, true);
            return Redirect("/");
        }

        public ActionResult Logoff()
        {

            Usuario u = usuarioModel.obterUsuarioPorLogin(User.Identity.Name);
            // Remover todos os perfis do mecanico
            foreach (Perfil p in perfilModel.listarPerfisPorUsuario(u.IdUsuario))
            {
                if (Roles.IsUserInRole(u.Login, p.Descricao))
                {
                    Roles.RemoveUserFromRole(u.Login, p.Descricao); // adiciona o mecanico
                }
            }
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
        public ActionResult Delete(int id)
        {
            Usuario u = usuarioModel.obterUsuario(id);
            usuarioModel.excluirUsuario(u);
            return RedirectToAction("Index");
        }
       

    }
}
