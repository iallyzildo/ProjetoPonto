using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;

namespace ProjetoPonto.Models
{
    public class PerfilModel
    {
        private pontoEntities db = new pontoEntities();

        public List<Perfil> todosPerfis()
        {
            var lista = from p in db.Perfil
                        select p;
            return lista.ToList();
        }

        public string adicionarPerfil(Perfil p)
        {
            string erro = null;
            try
            {
                db.Perfil.AddObject(p);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public Perfil obterPerfil(int idPerfil)
        {
            var lista = from p in db.Perfil
                        where p.IdPerfil == idPerfil
                        select p;
            return lista.ToList().FirstOrDefault();
        }



        public List<Perfil> listarPerfisPorUsuario(int idUsuario)
        {
            var lista = from p in db.Perfil
                        join pu in db.Usuario
                        on p.IdPerfil equals pu.IdPerfil
                        where pu.IdUsuario == idUsuario
                        select p;
            return lista.ToList();
        }
    }
}