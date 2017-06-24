using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Clinica.Models
{
    public class ContextoEF : DbContext
    {
        public ContextoEF() : base("name=StrConexao")
        {

        }

        public DbSet<Especie> Especies { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Animal> Animais { get; set; }
        public DbSet<Tratamento> Tratamentos { get; set; }
        public DbSet<Consulta> Consultas { get; set; }
        public DbSet<Exame> Exames { get; set; }
        public DbSet<Veterinario> Veterinarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}