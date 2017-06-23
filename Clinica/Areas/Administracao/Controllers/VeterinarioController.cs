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
    public class VeterinarioController : Controller
    {
        private ContextoEF db = new ContextoEF();

        public ActionResult ConsultarVeterinario(int? pagina, string NomeVeterinario = null)
        {
            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;
            var veterinarios = new object();
            if (!string.IsNullOrEmpty(NomeVeterinario))
            {
                veterinarios = db.Veterinarios
                    .Where(a => a.NomeVeterinario.ToUpper().Contains(NomeVeterinario.ToUpper()))
                    .OrderBy(a => a.NomeVeterinario).ToPagedList(numeroPagina, tamanhoPagina);

            }
            else
            {
                veterinarios = db.Veterinarios.OrderBy(a => a.NomeVeterinario).ToPagedList(numeroPagina, tamanhoPagina);
            }
            return View("Index", veterinarios);
        }

        // GET: Veterinario
        public ActionResult Index(string ordenacao, int? pagina)
        {
            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.NomeParam = string.IsNullOrEmpty(ordenacao) ? "Nome_desc" : "";
           
            var veterinario = from v in db.Veterinarios select v;

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            switch (ordenacao)
            {
                case "Nome_desc":
                    veterinario = veterinario.OrderByDescending(s => s.NomeVeterinario);
                break;
                default:
                    veterinario = veterinario.OrderBy(s => s.NomeVeterinario);
                break;
            }


            return View(veterinario.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: Veterinario/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veterinario veterinario = db.Veterinarios.Find(id);
            if (veterinario == null)
            {
                return HttpNotFound();
            }
            return View(veterinario);
        }

        // GET: Veterinario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Veterinario/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "VeterinarioID,NomeVeterinario,EnderecoVeterinario,TelefoneVeterinario")] Veterinario veterinario)
        {
            if (ModelState.IsValid)
            {
                db.Veterinarios.Add(veterinario);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(veterinario);
        }

        // GET: Veterinario/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veterinario veterinario = db.Veterinarios.Find(id);
            if (veterinario == null)
            {
                return HttpNotFound();
            }
            return View(veterinario);
        }

        // POST: Veterinario/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "VeterinarioID,NomeVeterinario,EnderecoVeterinario,TelefoneVeterinario")] Veterinario veterinario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(veterinario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(veterinario);
        }

        // GET: Veterinario/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Veterinario veterinario = db.Veterinarios.Find(id);
            if (veterinario == null)
            {
                return HttpNotFound();
            }
            return View(veterinario);
        }

        // POST: Veterinario/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Veterinario veterinario = db.Veterinarios.Find(id);
            db.Veterinarios.Remove(veterinario);
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
