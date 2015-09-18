using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ProjetoPonto.Entity;

namespace ProjetoPonto.Models
{
    public class ModeloMaquinaModel
    {
        private pontoEntities db = new pontoEntities();

        public List<ModeloMaquina> todosModeloMaquina()
        {
            var lista = from m in db.ModeloMaquina
                        select m;
            return lista.ToList();
        }
        public List<ModeloMaquina> PesquisaModeloMaquinas(string texto)
        {
            var lista = from m in db.ModeloMaquina
                        where m.Descricao.Contains(texto)
                        select m;
            return lista.ToList();
        }
        public string adicionarModeloMaquina(ModeloMaquina m)
        {
            string erro = null;
            try
            {
                db.ModeloMaquina.AddObject(m);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public ModeloMaquina obterModeloMaquina(int idModeloMaquina)
        {
            var lista = from m in db.ModeloMaquina
                        where m.IdModeloMaquina == idModeloMaquina
                        select m;
            return lista.ToList().FirstOrDefault();
        }
        public string editarModeloMaquina(ModeloMaquina m)
        {
            string erro = null;
            try
            {
                if (m.EntityState == System.Data.EntityState.Detached)
                {
                    db.ModeloMaquina.Attach(m);
                }
                db.ObjectStateManager.ChangeObjectState(m, System.Data.EntityState.Modified);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public string validarModeloMaquina(ModeloMaquina m)
        {
            string erro = null;

            if (m.Descricao == null || m.Descricao == "")
            {
                return "Descricao obrigatoria!";
            }


            if (m.IdMarca == 0)
            {
                return "Selecione a Marca!";
            }
            if (m.IdTipoMaquina == 0)
            {
                return "Selecione o tipo de maquina!";
            }

            return erro;
        }
        public string excluirModeloMaquina(ModeloMaquina m)
        {
            string erro = null;
            try
            {
                db.ModeloMaquina.DeleteObject(m);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                erro = ex.Message;
            }
            return erro;
        }
        public List<ModeloMaquina> listarModeloMaquinaPorTipoMaquina(int idTipoMaquina)
        {
            var lista = from m in db.ModeloMaquina
                        where m.IdTipoMaquina == idTipoMaquina
                        select m;

            return lista.ToList();
        }
    }
}