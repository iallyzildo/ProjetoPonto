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
        private PerfilUsuarioModel perfilUsuarioModel = new PerfilUsuarioModel();
      
        
        [Authorize]
        public ActionResult Index()
        {
            if (Roles.IsUserInRole(User.Identity.Name, "admin"))
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
            if (Roles.IsUserInRole(User.Identity.Name, "admin"))
            {
            Usuario u = new Usuario();
            int idFuncionario = 1;
            int idEmpresa = 1;
            if (id != 0)
            {
                u = usuarioModel.obterUsuario(id);
                idFuncionario = u.IdFuncionario;
                idEmpresa = u.IdEmpresa;
            }

            ViewBag.IdFuncionario = new SelectList(funcionarioModel.todosFuncionarios(), "IdFuncionario", "Nome", idFuncionario);
            ViewBag.IdEmpresa = new SelectList(empresaModel.todasEmpresas(), "IdEmpresa", "RazaoSocial", idEmpresa);

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
                // Testa se o usuario nao está na role associada ao banco
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
            usuarioModel.excluirUsuario(u);
            return RedirectToAction("Index");
        }
        public ActionResult ListaPerfilUsuario(int idUsuario)
        {
            List<PerfilUsuario> perfilUsuarioUsuarios = perfilUsuarioModel.listarPerfilUsaurioPorUsuario(idUsuario);
            Usuario u = usuarioModel.obterUsuario(idUsuario);
            ViewBag.IdUsuario = u.IdUsuario;
            ViewBag.LoginUsuario = u.Login;
            return View(perfilUsuarioUsuarios);
        }
        public ActionResult EditPerfilUsuario(int idUsuario, int idPerfilUsuario)
        {
            PerfilUsuario p = new PerfilUsuario();
            p.IdUsuario = idUsuario;

            int idPerfil = 2; //admin

            if (idPerfilUsuario != 0)
            {
                p = perfilUsuarioModel.obterPerfilUsuario(idPerfilUsuario);
                idPerfil= p.IdPerfil;
            }


            ViewBag.IdPerfil = new SelectList(perfilModel.todosPerfis(), "IdPerfil", "Descricao", idPerfil);
            return View(p);
        }
        [HttpPost]
        public ActionResult EditPerfilUsuario(PerfilUsuario p)
        {
            string erro = null;
            if (p.IdPerfilUsuario == 0)
            {
                erro = perfilUsuarioModel.adicionarPerfilUsuario(p);
            }
            else
            {
                erro = perfilUsuarioModel.editarPerfilUsaurio(p);
            }
            if (erro == null)
            {
                return RedirectToAction("ListaPerfilUsuario", new { idUsuario= p.IdUsuario });
            }
            else
            {
                ViewBag.Erro = erro;
                return View(p);
            }
        }
        public ActionResult DeletePerfilUsuario(int idPerfilUsuario)
        {
            PerfilUsuario p = perfilUsuarioModel.obterPerfilUsuario(idPerfilUsuario);
            perfilUsuarioModel.excluirPerfilUsuario(p);
            return RedirectToAction("ListaPerfilUsuario", new { idUsuario= p.IdUsuario });
        }

    }
}
