using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;

namespace ProjetoPonto.Models
{
    public class StatusOsModel
    {
        private pontoEntities db = new pontoEntities();

        public List<StatusOs> todosStatusOs()
        {
            var lista = from s in db.StatusOs
                        select s;
            return lista.ToList();
        }


        public StatusOs obterStatusOs(int idStatusOs)
        {
            var lista = from s in db.StatusOs
                        where s.IdStatusOs == idStatusOs
                        select s;
            return lista.ToList().FirstOrDefault();
        }


    }
}