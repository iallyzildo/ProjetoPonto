using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rotativa;
using Rotativa.Options;
using System.Web.Mvc;
using ProjetoPonto.Entity;
using ProjetoPonto.Models;
using System.Web.Security;

namespace ProjetoPonto.Controllers
{
    public class RelatoriosController : Controller
    {
        private PontoModel pontoModel = new PontoModel();

        public ActionResult ConsultaRegistros()
        {
           return View (pontoModel.todosPontos());
        }

        /*
         * Retorna um PDF diretamente no browser com as configurações padrões
         * ViewName é setado somente para utilizar o proprio Modelo anterior
         * Caso não queira setar o ViewName, você deve gerar a view com o mesmo nome da action
         */
        public ActionResult PDFPadrao()
        {
            var pdf = new ViewAsPdf(pontoModel.todosPontos());
            return pdf;
        }

        /*
         * Configura algumas propriedades do PDF, inclusive o nome do arquivo gerado,
         * Porem agora ele baixa o pdf ao inves de mostrar no browser
         */
        public ActionResult PDFConfigurado()
        {
            var pdf = new ViewAsPdf(pontoModel.todosPontos())
            {
                FileName = "NomeDoArquivoPDF.pdf",
                PageSize = Size.A4,
                IsGrayScale = true,
                CustomSwitches = "--print-media-type --header-center \"Redemaq Minas\"",
                PageMargins = new Margins { Bottom = 5, Left = 5, Right = 5, Top = 5 },
            };
            return pdf;
        }

        /*
         * Pode passar um modelo para a view que vai ser utilizada para gerar o PDF
         */
        public ActionResult Modelo()
        {
            //var modelo = new Usuario
            //{
            //    Login = "Ially Leandro",
            //};

            var pdf = new ViewAsPdf(pontoModel.todosPontos())
            {
                ViewName = "Modelo",
                CustomSwitches = "--print-media-type --header-center \"Redemaq Minas\""
                //Model = modelo
            };

            return pdf;
        }
    }
}
