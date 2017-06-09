using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinica.Models
{
    public class Tratamento
    {
        public int TratamentoID { get; set; }
        public int AnimalID { get; set; }
        public string Descricao { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public virtual Animal Animal { get; set; }
        public virtual ICollection<Consulta> Consultas { get; set; }
    }
}