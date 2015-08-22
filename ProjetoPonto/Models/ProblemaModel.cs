using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;


namespace ProjetoPonto.Models
{
    public class ProblemaModel
    {
        private pontoEntities db = new pontoEntities();

        public List<Problema> todosProblemas()
        {
            var lista = from p in db.Problema
                        select p;
            return lista.ToList();
        }
        public string adicionarProblema(Problema p)
        {
            string erro = null;
            try
            {
                db.Problema.AddObject(p);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public string editarProblema(Problema p)
        {
            string erro = null;
            try
            {
                if (p.EntityState == System.Data.EntityState.Detached)
                {
                    db.Problema.Attach(p);
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
        public Problema obterProblema(int id)
        {
            var lista = from p in db.Problema
                        where p.IdProblema == id
                        select p;
            return lista.ToList().FirstOrDefault();
        }
        public string excluirProblema(Problema p)
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
        public List<Problema> listarProblemaPorOs(int idOs)
        {
            var lista = from p in db.Problema
                        where p.IdOs == idOs
                        select p;

            return lista.ToList();
        }
        public string validarProblema(Problema p)
        {
            string erro = null;

            if (p.Descricao == null || p.Descricao == "")
            {
                return "Descricao obrigatória!";
            }

            if (p.IdOs == 0)
            {
                return "Selecione a OS!";
            }
            if (p.IdSecao == 0)
            {
                return "Selecione a OS!";
            }

            return erro;
        }
    }
}