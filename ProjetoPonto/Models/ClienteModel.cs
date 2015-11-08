using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;

namespace ProjetoPonto.Models
{
    public class ClienteModel
    {
        private pontoEntities db = new pontoEntities();

        public List<Cliente> todosClientes()
        {
            var lista = from c in db.Cliente
                        select c;
            return lista.ToList();
        }
        public List<Cliente> PesquisaClientes(string texto)
        {
            var lista = from c in db.Cliente
                        where c.Nome.Contains(texto)
                        select c;
            return lista.ToList();

        }
        public string adicionarCliente(Cliente c)
        {
            string erro = null;
            try
            {
                db.Cliente.AddObject(c);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }

        public Cliente obterCliente(int idCliente)
        {
            var lista = from c in db.Cliente
                        where c.IdCliente == idCliente
                        select c;
            return lista.ToList().FirstOrDefault();
        }

        public string editarCliente(Cliente c)
        {
            string erro = null;
            try
            {
                if (c.EntityState == System.Data.EntityState.Detached)
                {
                    db.Cliente.Attach(c);
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
        public string validarCliente(Cliente c)
        {
            string erro = null;

            if (c.Nome == null || c.Nome == "")
            {
                return "Nome obrigatório!";
            }
            if (c.Cpf == null || c.Cpf == "")
            {
                return "CPF obrigatório!";
            }

            return erro;
        }
        public string excluirCliente(Cliente c)
        {
            string erro = null;
            try
            {
                db.Cliente.DeleteObject(c);
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