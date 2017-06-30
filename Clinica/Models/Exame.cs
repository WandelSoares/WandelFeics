using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinica.Models
{
    public class Exame
    {
        public int ExameID { get; set; }
        public string DescricaoExame { get; set; }
        public virtual ICollection<Consulta> Consultas { get; set; }
    }
}