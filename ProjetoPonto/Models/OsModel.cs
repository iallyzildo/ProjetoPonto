using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;

namespace ProjetoPonto.Models
{
    public class OsModel
    {
        private pontoEntities db = new pontoEntities();

        public List<Os> todasOs()
        {
            var lista = from o in db.Os
                        select o;
            return lista.ToList();
        }
        public List<Os> todasOsAbertas()
        {
            var lista = from o in db.Os
                        join s in db.StatusOs
                        on o.IdStatusOs equals s.IdStatusOs
                        where s.IdStatusOs == 1
                        select o;
            return lista.ToList();
        }
        public int qtdOs()
        {
            int quantidadeOs= (from o in db.Os select o).Count();
            return quantidadeOs;
        }
        public string adicionarOs(Os o)
        {
            string erro = null;
            try
            {
                db.Os.AddObject(o);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public Os obterOs(int idOs)
        {
            var lista = from o in db.Os
                        where o.IdOs == idOs
                        select o;
            return lista.ToList().FirstOrDefault();
        }
        public string editarOs(Os o)
        {
            string erro = null;
            try
            {
                if (o.EntityState == System.Data.EntityState.Detached)
                {
                    db.Os.Attach(o);
                }
                db.ObjectStateManager.ChangeObjectState(o, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public string validarOs(Os o)
        {
            string erro = null;

            if (o.IdCliente == 0)
            {
                return "Selecione o Cliente!";
            }
            if (o.IdModeloMaquina == 0)
            {
                return "Selecione o Modelo da maquina!";
            }
            if (o.IdStatusOs == 0)
            {
                return "Selecione o Status da Os";
            }

            return erro;
        }
        public string excluirOs(Os o)
        {
            string erro = null;
            try
            {
                db.Os.DeleteObject(o);
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