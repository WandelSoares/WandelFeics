﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Clinica.Models;
using PagedList;

namespace Areas.Administracao.Controllers
{
    public class ConsultaController : Controller
    {
        private ContextoEF db = new ContextoEF();

        public ActionResult buscarConsulta(int? pagina, string Historico = null)
        {
            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;
            var consulta = new object();
            if (!string.IsNullOrEmpty(Historico))
            {
                consulta = db.Consultas
                    .Where(c => c.Historico.ToUpper().Contains(Historico.ToUpper()))
                    .OrderBy(c => c.Historico).ToPagedList(numeroPagina, tamanhoPagina);

            }
            else
            {
                consulta = db.Consultas.OrderBy(c => c.Historico).ToPagedList(numeroPagina, tamanhoPagina);
            }
            return View("Index", consulta);
        }

        // GET: Consulta
        public ActionResult Index(string ordenacao, int? pagina)
        {
            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.DataParam = string.IsNullOrEmpty(ordenacao) ? "Data_desc" : "";
            ViewBag.VeterinarioParam = ordenacao == "Veterinario" ? "Veterinario_desc" : "Veterinario";

            var consulta = from c in db.Consultas select c;

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            switch (ordenacao)
            {
                case "Data_desc":
                    consulta = consulta.OrderByDescending(s => s.DataConsulta);
                    break;
                case "Veterinario":
                    consulta = consulta.OrderBy(s => s.VeterinarioID);
                    break;
                case "Veterinario_desc":
                    consulta = consulta.OrderByDescending(s => s.VeterinarioID);
                    break;
                default:
                    consulta = consulta.OrderBy(s => s.DataConsulta);
                    break;
            }


            var consultas = db.Consultas.Include(c => c.Tratamento).Include(c => c.Veterinario);
            return View(consulta.ToPagedList(numeroPagina, tamanhoPagina));
        }


        public ActionResult ObterConsultasPorExame(int id, int? pagina)
        {
            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            var consulta = from c in db.Consultas where c.Exame.ExameID == id select c;
            return View("Index", consulta.OrderBy(x=>x.Exame.ExameID).ToPagedList(numeroPagina, tamanhoPagina));
        }


        // GET: Consulta/Details/5
        public ActionResult Detalhes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consulta consulta = db.Consultas.Find(id);
            if (consulta == null)
            {
                return HttpNotFound();
            }
            return View(consulta);
        }

        // GET: Consulta/Create
        public ActionResult Cadastrar()
        {
            ViewBag.TratamentoID = new SelectList(db.Tratamentos, "TratamentoID", "Descricao");
            ViewBag.VeterinarioID = new SelectList(db.Veterinarios, "VeterinarioID", "NomeVeterinario");
            ViewBag.AnimalID = new SelectList(db.Animais, "AnimalID", "NomeAnimal");
            ViewBag.ExameID = new SelectList(db.Exames, "ExameID", "DescricaoExame");

            return View();
        }

        // POST: Consulta/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Consulta consulta)
        {
            if (ModelState.IsValid)
            {
                db.Consultas.Add(consulta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TratamentoID = new SelectList(db.Tratamentos, "TratamentoID", "Descricao", consulta.TratamentoID);
            ViewBag.VeterinarioID = new SelectList(db.Veterinarios, "VeterinarioID", "NomeVeterinario", consulta.VeterinarioID);
            ViewBag.ExameID = new SelectList(db.Exames, "ExameID", "DescricaoExame");

            return View(consulta);
        }

        // GET: Consulta/Edit/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consulta consulta = db.Consultas.Find(id);
            if (consulta == null)
            {
                return HttpNotFound();
            }
            ViewBag.TratamentoID = new SelectList(db.Tratamentos, "TratamentoID", "Descricao", consulta.TratamentoID);
            ViewBag.VeterinarioID = new SelectList(db.Veterinarios, "VeterinarioID", "NomeVeterinario", consulta.VeterinarioID);
            ViewBag.AnimalID = new SelectList(db.Animais, "AnimalID", "NomeAnimal", consulta.AnimalID);
            ViewBag.ExameID = new SelectList(db.Exames, "ExameID", "DescricaoExame");

            return View(consulta);
        }

        // POST: Consulta/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(Consulta consulta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consulta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TratamentoID = new SelectList(db.Tratamentos, "TratamentoID", "Descricao", consulta.TratamentoID);
            ViewBag.VeterinarioID = new SelectList(db.Veterinarios, "VeterinarioID", "NomeVeterinario", consulta.VeterinarioID);
            ViewBag.AnimalID = new SelectList(db.Animais, "AnimalID", "NomeAnimal", consulta.AnimalID);
            return View(consulta);
        }

        // GET: Consulta/Delete/5
        public ActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Consulta consulta = db.Consultas.Find(id);
            if (consulta == null)
            {
                return HttpNotFound();
            }
            return View(consulta);
        }

        // POST: Consulta/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmaExclusao(int id)
        {
            Consulta consulta = db.Consultas.Find(id);
            db.Consultas.Remove(consulta);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
