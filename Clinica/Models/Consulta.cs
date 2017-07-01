using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Clinica.Models
{
    public class Consulta
    {
        public int ConsultaID { get; set; }
        public int TratamentoID { get; set; }
        public int VeterinarioID { get; set; }
        public int ExameID { get; set; }
        [DisplayFormat(DataFormatString="{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DataConsulta { get; set; }
        public string Historico { get; set; }
        public virtual Tratamento Tratamento { get; set; }
        public virtual Exame Exame { get; set; }
        public virtual Veterinario Veterinario { get; set; }
        public int? AnimalID { get; set; }
        public virtual Animal Animal { get; set; }
    }
}