using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;

namespace ProjetoPonto.Models
{
    public class SecaoProblemaModel
    {

        private pontoEntities db = new pontoEntities();

        public List<SecaoProblema> todasSecaoProblema()
        {
            var lista = from s in db.SecaoProblema
                        select s;
            return lista.ToList();
        }
        public string adicionarSecaoProblema(SecaoProblema s)
        {
            string erro = null;
            try
            {
                db.SecaoProblema.AddObject(s);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public SecaoProblema obterSecaoProblema(int idSecao)
        {
            var lista = from s in db.SecaoProblema
                        where s.IdSecao == idSecao
                        select s;
            return lista.ToList().FirstOrDefault();
        }
        public string editarSecaoProblema(SecaoProblema s)
        {
            string erro = null;
            try
            {
                if (s.EntityState == System.Data.EntityState.Detached)
                {
                    db.SecaoProblema.Attach(s);
                }
                db.ObjectStateManager.ChangeObjectState(s, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public string excluirSecaoProblema(SecaoProblema s)
        {
            string erro = null;
            try
            {
                db.SecaoProblema.DeleteObject(s);
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