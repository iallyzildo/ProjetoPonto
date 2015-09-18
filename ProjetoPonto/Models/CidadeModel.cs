using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;


namespace ProjetoPonto.Models
{
    public class CidadeModel
    {
        private pontoEntities db = new pontoEntities();

        public List<Cidade> todasCidades()
        {
            var lista = from c in db.Cidade
                        select c;
            return lista.ToList();
        }
        public List<Cidade> PesquisaCidades(string texto)
        {
            var lista = from c in db.Cidade
                        where c.Descricao.Contains(texto)
                        select c;
            return lista.ToList();

        }
        public string adicionarCidade(Cidade c)
        {
            string erro = null;
            try
            {
                db.Cidade.AddObject(c);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public string editarCidade(Cidade c)
        {
            string erro = null;
            try
            {
                if (c.EntityState == System.Data.EntityState.Detached)
                {
                    db.Cidade.Attach(c);
                }
                db.ObjectStateManager.ChangeObjectState(c, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public Cidade obterCidade(int id)
        {
            var lista = from c in db.Cidade
                        where c.IdCidade == id
                        select c;
            return lista.ToList().FirstOrDefault();
        }
        public string excluirCidade(Cidade c)
        {
            string erro = null;
            try
            {
                db.DeleteObject(c);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public List<Cidade> listarCidadePorEstado(int idEstado)
        {
            var lista = from c in db.Cidade
                        where c.IdEstado == idEstado
                        select c;

            return lista.ToList();
        }
        public string validarCidade(Cidade c)
        {
            string erro = null;

            if (c.Descricao == null || c.Descricao == "")
            {
                return "Descricao obrigatória!";
            }

            if (c.IdEstado == 0)
            {
                return "Selecione o Estado!";
            }

            return erro;
        }
    }
}