using Clinica.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Clinica.DiagramadeClasses
{
    public static class DiagramaDeClasse
    {
        public static void GerarDiagrama()
        {
            using (var ctx = new ContextoEF())
            {
                using (var writer = new XmlTextWriter(@"C:\Users\wande\Source\Repos\WandelFeics\Clinica\DiagramadeClasses\Model.edmx", Encoding.Default))
                {
                    EdmxWriter.WriteEdmx(ctx, writer);
                }
            }
        }
    }
}