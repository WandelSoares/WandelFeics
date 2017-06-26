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
    public class TratamentoController : Controller
    {
        private ContextoEF db = new ContextoEF();

        public ActionResult ConsultarTratamento(int? pagina, string Descricao = null)
        {
            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;
            var tratamento = new object();
            if (!string.IsNullOrEmpty(Descricao))
            {
                tratamento = db.Tratamentos
                    .Where(a => a.Descricao.ToUpper().Contains(Descricao.ToUpper()))
                    .OrderBy(a => a.Descricao).ToPagedList(numeroPagina, tamanhoPagina);

            }
            else
            {
                tratamento = db.Tratamentos.OrderBy(a => a.Descricao).ToPagedList(numeroPagina, tamanhoPagina);
            }
            return View("Index", tratamento);
        }

        // GET: Tratamento
        public ActionResult Index(string ordenacao, int? pagina)
        {
            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.DescricaoParam = string.IsNullOrEmpty(ordenacao) ? "Tipo_desc" : "";
            ViewBag.NomeParam = ordenacao == "Nome" ? "Nome_desc" : "Nome";

            var tratamento = from t in db.Tratamentos select t;

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            switch (ordenacao)
            {
                case "Tipo_desc":
                    tratamento = tratamento.OrderByDescending(s => s.Descricao);
                    break;
                case "Nome":
                    tratamento = tratamento.OrderBy(s => s.AnimalID);
                    break;
                case "Nome_desc":
                    tratamento = tratamento.OrderByDescending(s => s.AnimalID);
                    break;
                default:
                    tratamento = tratamento.OrderBy(s => s.Descricao);
                    break;
            }


            var tratamentos = db.Tratamentos.Include(t => t.Animal);
            return View(tratamento.ToPagedList(numeroPagina, tamanhoPagina));
        }

        // GET: Tratamento/Details/5
        public ActionResult Detalhes(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tratamento tratamento = db.Tratamentos.Find(id);
            if (tratamento == null)
            {
                return HttpNotFound();
            }
            return View(tratamento);
        }

        // GET: Tratamento/Create
        public ActionResult Cadastrar()
        {
            ViewBag.AnimalID = new SelectList(db.Animais, "AnimalID", "NomeAnimal");
            return View();
        }

        // POST: Tratamento/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar([Bind(Include = "TratamentoID,AnimalID,Descricao,DataInicio,DataFim")] Tratamento tratamento)
        {
            if (ModelState.IsValid)
            {
                db.Tratamentos.Add(tratamento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AnimalID = new SelectList(db.Animais, "AnimalID", "NomeAnimal", tratamento.AnimalID);
            return View(tratamento);
        }

        // GET: Tratamento/Edit/5
        public ActionResult Editar(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tratamento tratamento = db.Tratamentos.Find(id);
            if (tratamento == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnimalID = new SelectList(db.Animais, "AnimalID", "NomeAnimal", tratamento.AnimalID);
            return View(tratamento);
        }

        // POST: Tratamento/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar([Bind(Include = "TratamentoID,AnimalID,Descricao,DataInicio,DataFim")] Tratamento tratamento)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tratamento).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AnimalID = new SelectList(db.Animais, "AnimalID", "NomeAnimal", tratamento.AnimalID);
            return View(tratamento);
        }

        // GET: Tratamento/Delete/5
        public ActionResult Excluir(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tratamento tratamento = db.Tratamentos.Find(id);
            if (tratamento == null)
            {
                return HttpNotFound();
            }
            return View(tratamento);
        }

        // POST: Tratamento/Delete/5
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmaExclusao(int id)
        {
            Tratamento tratamento = db.Tratamentos.Find(id);
            db.Tratamentos.Remove(tratamento);
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
