using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinica.Models
{
    public class Veterinario
    {
        public int VeterinarioID { get; set; }
        public string NomeVeterinario { get; set; }
        public string EnderecoVeterinario { get; set; }
        public string TelefoneVeterinario { get; set; }
        public virtual ICollection<Consulta> Consultas { get; set; }

    }
}