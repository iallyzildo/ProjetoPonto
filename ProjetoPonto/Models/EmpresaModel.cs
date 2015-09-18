using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;

namespace ProjetoPonto.Models
{
    public class EmpresaModel
    {
        private pontoEntities db = new pontoEntities();

        public List<Empresa> todasEmpresas()
        {
            var lista = from e in db.Empresa
                        select e;
            return lista.ToList();
        }
        public List<Empresa> PesquisaEmpresas(string texto)
        {
            var lista = from e in db.Empresa
                        where e.RazaoSocial.Contains(texto)
                        select e;
            return lista.ToList();

        }
        public string adicionarEmpresa(Empresa e)
        {
            string erro = null;
            try
            {
                db.Empresa.AddObject(e);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public Empresa obterEmpresa(int idEmpresa)
        {
            var lista = from e in db.Empresa
                        where e.IdEmpresa == idEmpresa
                        select e;
            return lista.ToList().FirstOrDefault();
        }
        public string editarEmpresa(Empresa e)
        {
            string erro = null;
            try
            {
                if (e.EntityState == System.Data.EntityState.Detached)
                {
                    db.Empresa.Attach(e);
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
        public string validarEmpresa(Empresa e)
        {
            string erro = null;

            if (e.RazaoSocial == null || e.RazaoSocial == "")
            {
                return "Razao Social obrigatória!";
            }
            if (e.CNPJ == null || e.CNPJ == "")
            {
                return "CNPJ obrigatório!";
            }

            if (e.IdCidade == 0)
            {
                return "Selecione a Cidade!";
            }

            return erro;
        }
        public string excluirEmpresa(Empresa e)
        {
            string erro = null;
            try
            {
                db.Empresa.DeleteObject(e);
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