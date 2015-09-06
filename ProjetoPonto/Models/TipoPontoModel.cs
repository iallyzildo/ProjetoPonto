using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;

namespace ProjetoPonto.Models
{
    public class TipoPontoModel
    {

        private pontoEntities db = new pontoEntities();

        public List<TipoPonto> todosTipoPonto()
        {
            var lista = from t in db.TipoPonto
                        select t;
            return lista.ToList();
        }
        public string adicionarTipoPonto(TipoPonto t)
        {
            string erro = null;
            try
            {
                db.TipoPonto.AddObject(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public TipoPonto obterTipoPonto(int idTipoPonto)
        {
            var lista = from t in db.TipoPonto
                        where t.IdTipoPonto == idTipoPonto
                        select t;
            return lista.ToList().FirstOrDefault();
        }
        public string editarTipoPonto(TipoPonto t)
        {
            string erro = null;
            try
            {
                if (t.EntityState == System.Data.EntityState.Detached)
                {
                    db.TipoPonto.Attach(t);
                }
                db.ObjectStateManager.ChangeObjectState(t, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public string excluirTipoPonto(TipoPonto t)
        {
            string erro = null;
            try
            {
                db.TipoPonto.DeleteObject(t);
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