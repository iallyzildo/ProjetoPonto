using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;


namespace ProjetoPonto.Models
{
    public class SolucaoModel
    {
        private pontoEntities db = new pontoEntities();

        public List<Solucao> todasSolucoes()
        {
            var lista = from s in db.Solucao
                        select s;
            return lista.ToList();
        }
        public string adicionarSolucao(Solucao s)
        {
            string erro = null;
            try
            {
                db.Solucao.AddObject(s);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public string editarSolucao(Solucao s)
        {
            string erro = null;
            try
            {
                if (s.EntityState == System.Data.EntityState.Detached)
                {
                    db.Solucao.Attach(s);
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
        public Solucao obterSolucao(int id)
        {
            var lista = from s in db.Solucao
                        where s.IdSolucao== id
                        select s;
            return lista.ToList().FirstOrDefault();
        }
        public string excluirSolucao(Solucao s)
        {
            string erro = null;
            try
            {
                db.DeleteObject(s);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public List<Solucao> listarSolucaoPorProblema(int idProblema)
        {
            var lista = from s in db.Solucao
                        where s.IdProblema == idProblema
                        select s;

            return lista.ToList();
        }
        public string validarSolucao(Solucao s)
        {
            string erro = null;

            if (s.Descricao == null || s.Descricao == "")
            {
                return "Descricao obrigatória!";
            }

            return erro;
        }
    }
}