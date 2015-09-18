using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;

namespace ProjetoPonto.Models
{
    public class TipoMaquinaModel
    {
        private pontoEntities db = new pontoEntities();

        public List<TipoMaquina> todosTipoMaquina()
        {
            var lista = from t in db.TipoMaquina
                        select t;
            return lista.ToList();
        }
        public List<TipoMaquina> PesquisaTipoMaquinas(string texto)
        {
            var lista = from t in db.TipoMaquina
                        where t.Descricao.Contains(texto)
                        select t;
            return lista.ToList();
        }
        public string adicionarTipoMaquina(TipoMaquina t)
        {
            string erro = null;
            try
            {
                db.TipoMaquina.AddObject(t);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public TipoMaquina obterTipoMaquina(int idTipoMaquina)
        {
            var lista = from t in db.TipoMaquina
                        where t.IdTipoMaquina == idTipoMaquina
                        select t;
            return lista.ToList().FirstOrDefault();
        }
        public string editarTipoMaquina(TipoMaquina t)
        {
            string erro = null;
            try
            {
                if (t.EntityState == System.Data.EntityState.Detached)
                {
                    db.TipoMaquina.Attach(t);
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
        public string excluirTipoMaquina(TipoMaquina t)
        {
            string erro = null;
            try
            {
                db.TipoMaquina.DeleteObject(t);
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