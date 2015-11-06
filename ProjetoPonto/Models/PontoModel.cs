using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;

namespace ProjetoPonto.Models
{
    public class PontoModel
    {

        private pontoEntities db = new pontoEntities();

        public List<Ponto> todosPontos()
        {
            var lista = from p in db.Ponto
                        select p;
            return lista.ToList();
        }

        public List<Ponto> todosPontosFechados()
        {
            var lista = from p in db.Ponto
                        where p.HoraFinal != null
                        select p;
            return lista.ToList();
        }

        public List<Ponto> todosPontosAbertos()
        {
            var lista = from p in db.Ponto
                        where p.HoraFinal == null
                        select p;
            return lista.ToList();
        }

        public List<Ponto> PesquisaPontosOs(string os)
        {
            var lista = from p in db.Ponto
                        where p.Os.NumeroOs == (os)
                        where p.HoraFinal != null
                        select p;
            return lista.ToList();

        }

        public int qtdPontos()
        {
            int quantidadePonto = (from p in db.Ponto where p.HoraFinal != null select p).Count();
            return quantidadePonto;
        }
        public int qtdPontosAbertos()
        {
            int quantidadePontoAbertos = (from p in db.Ponto where p.HoraFinal == null select p).Count();
            return quantidadePontoAbertos;
        }
        public List<Ponto> todosPontosAbertosPorUsuario(string login)
        {
            var lista = from p in db.Ponto
                        join u in db.Usuario 
                        on p.IdUsuario equals u.IdUsuario
                        where u.Login == login 
                        where p.HoraFinal == null
                        select p;
            return lista.ToList();
        }

        public string adicionarPonto(Ponto p)
        {
            string erro = null;
            try
            {
                db.Ponto.AddObject(p);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public Ponto obterPonto(int idPonto)
        {
            var lista = from p in db.Ponto
                        where p.IdPonto == idPonto
                        select p;
            return lista.ToList().FirstOrDefault();
        }
        public string editarPonto(Ponto p)
        {
            string erro = null;
            try
            {
                if (p.EntityState == System.Data.EntityState.Detached)
                {
                    db.Ponto.Attach(p);
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
        public string excluirPonto(Ponto p)
        {
            string erro = null;
            try
            {
                db.Ponto.DeleteObject(p);
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