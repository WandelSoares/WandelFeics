using System;
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
    public class EspecieController : Controller
    {
        private ContextoEF db = new ContextoEF();

        public ActionResult ConsultarEspecie(int? pagina, string NomeEspecie = null)
        {
            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;
            var especies = new object();
            if (!string.IsNullOrEmpty(NomeEspecie))
            {
                especies = db.Especies
                    .Where(t => t.NomeEspecie.ToUpper().Contains(NomeEspecie.ToUpper()))
                    .OrderBy(t => t.NomeEspecie).ToPagedList(numeroPagina, tamanhoPagina);

            }
            else
            {
                especies = db.Especies.OrderBy(t => t.NomeEspecie).ToPagedList(numeroPagina, tamanhoPagina);
            }
            return View("Index", especies);
        }

        // GET: Especie
        public ActionResult Index(string ordenacao, int? pagina)
        {
            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.NomeParam = string.IsNullOrEmpty(ordenacao) ? "Nome_desc" : "";

            var especie = from e in db.Especies select e;

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            switch (ordenacao)
            {
                case "Nome_desc":
                    especie = especie.OrderByDescending(s => s.NomeEspecie);
                    break;
                default:
                    especie = especie.OrderBy(s => s.NomeEspecie);
                    break;
            }

            return View(especie.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: Especie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especie especie = db.Especies.Find(id);
            if (especie == null)
            {
                return HttpNotFound();
            }
            return View(especie);
        }

        // GET: Especie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Especie/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EspecieID,NomeEspecie")] Especie especie)
        {
            if (ModelState.IsValid)
            {
                db.Especies.Add(especie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(especie);
        }

        // GET: Especie/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especie especie = db.Especies.Find(id);
            if (especie == null)
            {
                return HttpNotFound();
            }
            return View(especie);
        }

        // POST: Especie/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EspecieID,NomeEspecie")] Especie especie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(especie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(especie);
        }

        // GET: Especie/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especie especie = db.Especies.Find(id);
            if (especie == null)
            {
                return HttpNotFound();
            }
            return View(especie);
        }

        // POST: Especie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Especie especie = db.Especies.Find(id);
            db.Especies.Remove(especie);
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
