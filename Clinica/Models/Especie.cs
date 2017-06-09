using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinica.Models
{
    public class Especie
    {
        public int EspecieID { get; set; }
        public string NomeEspecie { get; set; }
        public virtual ICollection<Animal> Animais { get; set; }

    }
}