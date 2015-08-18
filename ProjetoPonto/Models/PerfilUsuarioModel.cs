using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;

namespace ProjetoPonto.Models
{
    public class PerfilUsuarioModel
    {

        private pontoEntities db = new pontoEntities();

        public List<PerfilUsuario> todosPerfilUsuario()
        {
            var lista = from p in db.PerfilUsuario
                        select p;
            return lista.ToList();
        }

        public string adicionarPerfilUsuario(PerfilUsuario p)
        {
            string erro = null;
            try
            {
                db.PerfilUsuario.AddObject(p);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public string editarPerfilUsaurio(PerfilUsuario p)
        {
            string erro = null;
            try
            {
                if (p.EntityState == System.Data.EntityState.Detached)
                {
                    db.PerfilUsuario.Attach(p);
                }
                db.ObjectStateManager.ChangeObjectState(p, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public PerfilUsuario obterPerfilUsuario(int id)
        {
            var lista = from p in db.PerfilUsuario
                        where p.IdPerfilUsuario == id
                        select p;
            return lista.ToList().FirstOrDefault();
        }

        public string excluirPerfilUsuario(PerfilUsuario p)
        {
            string erro = null;
            try
            {
                db.DeleteObject(p);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public List<PerfilUsuario> listarPerfilUsaurioPorUsuario(int idUsuario)
        {
            var lista = from p in db.PerfilUsuario
                        where p.IdUsuario == idUsuario
                        select p;

            return lista.ToList();
        }

    }
}