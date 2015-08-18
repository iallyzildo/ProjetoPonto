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

    }
}