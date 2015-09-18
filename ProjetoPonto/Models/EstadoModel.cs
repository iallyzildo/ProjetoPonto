using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;

namespace ProjetoPonto.Models
{
    public class EstadoModel
    {

        private pontoEntities db = new pontoEntities();

        public List<Estado> todosEstados()
        {
            var lista = from e in db.Estado
                        select e;
            return lista.ToList();
        }
        public List<Estado> PesquisaEstados(string texto)
        {
            var lista = from e in db.Estado
                        where e.Descricao.Contains(texto)
                        select e;
            return lista.ToList();

        }
        public string adicionarEstado(Estado e)
        {
            string erro = null;
            try
            {
                db.Estado.AddObject(e);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public Estado obterEstado(int idEstado)
        {
            var lista = from e in db.Estado
                        where e.IdEstado == idEstado
                        select e;
            return lista.ToList().FirstOrDefault();
        }
        public string editarEstado(Estado e)
        {
            string erro = null;
            try
            {
                if (e.EntityState == System.Data.EntityState.Detached)
                {
                    db.Estado.Attach(e);
                }
                db.ObjectStateManager.ChangeObjectState(e, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public string excluirEstado(Estado e)
        {
            string erro = null;
            try
            {
                db.Estado.DeleteObject(e);
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