using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Clinica.Models
{
    public class Animal
    {
        public int AnimalID { get; set; }
        public int ClienteID { get; set; }
        public int EspecieID { get; set; }
        public string NomeAnimal { get; set; }
        public string IdadeAnimal { get; set; }
        public string SexoAnimal { get; set; }
        public virtual Cliente Cliente { get; set; }
        public virtual Especie Especie { get; set; }
        public virtual ICollection<Tratamento> Tratamentos { get; set; }




    }
}