using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;
using System.Web.Security;
using System.Web.Mvc;


namespace ProjetoPonto.Models
{
    public class UsuarioModel
    {
        private pontoEntities db = new pontoEntities();
        private PerfilModel perfilModel = new PerfilModel();
       
     
        public string  EnviarEmail(string mensagem)
        {
            string mensagemEmail = mensagem;
            return mensagemEmail;

        }

        public List<Usuario> todosUsuarios()
        {
            var lista = from u in db.Usuario
                        select u;
            return lista.ToList();
        }

        public List<Usuario> PesquisaUsuarios(string texto)
        {
            var lista = from u in db.Usuario
                        where u.Login.Contains(texto)
                        select u;
            return lista.ToList();

        }
        public int qtdUsuarios()
        {
            int quantidadeUsuarios = (from p in db.Usuario select p).Count();
            return quantidadeUsuarios;
        }

        public int qtdUsuariosMecanicos()
        {
            int quantidadeUsuariosMecanicos = (from u in db.Usuario where u.IdPerfil == 3 select u).Count();
            return quantidadeUsuariosMecanicos;
        }

        public int qtdUsuariosAdministradores()
        {
            int quantidadeUsuariosAdministradores = (from p in db.Usuario where p.IdPerfil == 1 select p).Count();
            return quantidadeUsuariosAdministradores;
        }

        public int qtdUsuariosGerentes()
        {
            int quantidadeUsuariosGerentes = (from p in db.Usuario where p.IdPerfil == 2 select p).Count();
            return quantidadeUsuariosGerentes;
        }

        

        public string adicionarUsuario(Usuario u)
        {
            string erro = null;
            try
            {
                db.Usuario.AddObject(u);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public Usuario obterUsuario(int idUsuario)
        {
            var lista = from u in db.Usuario
                        where u.IdUsuario == idUsuario
                        select u;
            return lista.ToList().FirstOrDefault();
        }
        public Usuario obterUsuarioPorLogin(string login)
        {
            var lista = from u in db.Usuario
                        where u.Login == login
                        select u;
            return lista.ToList().FirstOrDefault();
        }
        public string editarUsuario(Usuario u)
        {
            string erro = null;
            try
            {
                if (u.EntityState == System.Data.EntityState.Detached)
                {
                    db.Usuario.Attach(u);
                }
                db.ObjectStateManager.ChangeObjectState(u, System.Data.EntityState.Modified);
                db.SaveChanges();
                Roles.DeleteCookie();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public string validarUsuario(Usuario u)
        {
            string erro = null;

            if (u.Login == null || u.Login == "")
            {
                return "Login obrigatória!";
            }
            if (u.Senha == null || u.Senha == "")
            {
                return "Senha obrigatoria!";
            }
            if (u.Email == null || u.Email == "")
            {
                return "Email obrigatorio!";
            }

            if (u.IdFuncionario == 0)
            {
                return "Selecione o Funcionario!";
            }
            if (u.IdEmpresa == 0)
            {
                return "Selecione a Empresa!";
            }

            return erro;
        }
        public string excluirUsuario(Usuario u)
        {
            string erro = null;
            try
            {
                db.Usuario.DeleteObject(u);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
    }
}