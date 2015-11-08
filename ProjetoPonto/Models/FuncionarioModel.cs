using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;

namespace ProjetoPonto.Models
{
    public class FuncionarioModel
    {
        private pontoEntities db = new pontoEntities();

        public List<Funcionario> todosFuncionarios()
        {
            var lista = from f in db.Funcionario
                        select f;
            return lista.ToList();
        }
         
        public List<Funcionario> PesquisaFuncionarios(string texto)
        {
            var lista = from u in db.Funcionario where u.Nome.Contains(texto)
                        select u;
            return lista.ToList();
            
        }
        public string adicionarFuncionario(Funcionario f)
        {
            string erro = null;
            try
            {
                db.Funcionario.AddObject(f);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public Funcionario obterFuncionario(int idFuncionario)
        {
            var lista = from f in db.Funcionario
                        where f.IdFuncionario == idFuncionario
                        select f;
            return lista.ToList().FirstOrDefault();
        }
        public string editarFuncionario(Funcionario f)
        {
            string erro = null;
            try
            {
                if (f.EntityState == System.Data.EntityState.Detached)
                {
                    db.Funcionario.Attach(f);
                }
                db.ObjectStateManager.ChangeObjectState(f, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public string validarFuncionario(Funcionario f)
        {
            string erro = null;

            if (f.Nome == null || f.Nome == "")
            {
                return "Nome obrigatório!";
            }
            if (f.Cpf == null || f.Cpf == "")
            {
                return "CPF obrigatório!";
            }

            return erro;
        }
        public string excluirFuncionario(Funcionario f)
        {
            string erro = null;
            try
            {
                db.Funcionario.DeleteObject(f);
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