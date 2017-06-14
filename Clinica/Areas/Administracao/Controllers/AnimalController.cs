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
    public class AnimalController : Controller
    {
        private ContextoEF db = new ContextoEF();

        // GET: Animal
        public ActionResult Index(string ordenacao, int? pagina)
        {
            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.NomeParam = string.IsNullOrEmpty(ordenacao) ? "Nome_desc" : "";
            ViewBag.especieParam = ordenacao == "Especie" ? "Especie_desc" : "Especie";

            var animal = from a in db.Animais select a;

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            switch (ordenacao)
            {
                case "Nome_desc":
                    animal = animal.OrderByDescending(s => s.NomeAnimal);
                    break;
                case "Especie":
                    animal = animal.OrderBy(s => s.EspecieID);
                    break;
                case "Especie_desc":
                    animal = animal.OrderByDescending(s => s.EspecieID);
                    break;
                default:
                    animal = animal.OrderBy(s => s.NomeAnimal);
                    break;
            }

            
        var animais = db.Animais.Include(a => a.Cliente).Include(a => a.Especie);
            return View(animal.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: Animal/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animais.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // GET: Animal/Create
        public ActionResult Create()
        {
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome");
            ViewBag.EspecieID = new SelectList(db.Especies, "EspecieID", "NomeEspecie");
            return View();
        }

        // POST: Animal/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnimalID,ClienteID,EspecieID,NomeAnimal,IdadeAnimal,SexoAnimal")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                db.Animais.Add(animal);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", animal.ClienteID);
            ViewBag.EspecieID = new SelectList(db.Especies, "EspecieID", "NomeEspecie", animal.EspecieID);
            return View(animal);
        }

        // GET: Animal/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animais.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", animal.ClienteID);
            ViewBag.EspecieID = new SelectList(db.Especies, "EspecieID", "NomeEspecie", animal.EspecieID);
            return View(animal);
        }

        // POST: Animal/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnimalID,ClienteID,EspecieID,NomeAnimal,IdadeAnimal,SexoAnimal")] Animal animal)
        {
            if (ModelState.IsValid)
            {
                db.Entry(animal).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClienteID = new SelectList(db.Clientes, "ClienteID", "Nome", animal.ClienteID);
            ViewBag.EspecieID = new SelectList(db.Especies, "EspecieID", "NomeEspecie", animal.EspecieID);
            return View(animal);
        }

        // GET: Animal/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Animal animal = db.Animais.Find(id);
            if (animal == null)
            {
                return HttpNotFound();
            }
            return View(animal);
        }

        // POST: Animal/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Animal animal = db.Animais.Find(id);
            db.Animais.Remove(animal);
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
