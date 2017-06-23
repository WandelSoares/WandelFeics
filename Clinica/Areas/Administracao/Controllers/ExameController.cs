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
    public class ExameController : Controller
    {
        private ContextoEF db = new ContextoEF();

        public ActionResult ConsultarExame(int? pagina, string DescricaoExame = null)
        {
            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;
            var exame = new object();
            if (!string.IsNullOrEmpty(DescricaoExame))
            {
                exame = db.Exames
                    .Where(a => a.DescricaoExame.ToUpper().Contains(DescricaoExame.ToUpper()))
                    .OrderBy(a => a.DescricaoExame).ToPagedList(numeroPagina, tamanhoPagina);

            }
            else
            {
                exame = db.Exames.OrderBy(a => a.DescricaoExame).ToPagedList(numeroPagina, tamanhoPagina);
            }
            return View("Index", exame);
        }

        // GET: Exame
        public ActionResult Index(string ordenacao, int? pagina)
        {
            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.DescricaoParam = string.IsNullOrEmpty(ordenacao) ? "Descricao_desc" : "";

            var exame = from e in db.Exames select e;

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            switch (ordenacao)
            {
                case "Descricao_desc":
                    exame = exame.OrderByDescending(s => s.DescricaoExame);
                    break;
                default:
                    exame = exame.OrderBy(s => s.DescricaoExame);
                    break;
            }

            return View(exame.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: Exame/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exame exame = db.Exames.Find(id);
            if (exame == null)
            {
                return HttpNotFound();
            }
            return View(exame);
        }

        // GET: Exame/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Exame/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ExameID,ConsultaID,DescricaoExame")] Exame exame)
        {
            if (ModelState.IsValid)
            {
                db.Exames.Add(exame);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(exame);
        }

        // GET: Exame/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exame exame = db.Exames.Find(id);
            if (exame == null)
            {
                return HttpNotFound();
            }
            return View(exame);
        }

        // POST: Exame/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ExameID,ConsultaID,DescricaoExame")] Exame exame)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exame).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(exame);
        }

        // GET: Exame/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exame exame = db.Exames.Find(id);
            if (exame == null)
            {
                return HttpNotFound();
            }
            return View(exame);
        }

        // POST: Exame/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exame exame = db.Exames.Find(id);
            db.Exames.Remove(exame);
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
