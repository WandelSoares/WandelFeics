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
    public class ClienteController : Controller
    {
        private ContextoEF db = new ContextoEF();

        public ActionResult ConsultarCliente(int? pagina, string Nome = null)
        {
            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;
            var cliente = new object();
            if (!string.IsNullOrEmpty(Nome))
            {
                cliente = db.Clientes
                    .Where(t => t.Nome.ToUpper().Contains(Nome.ToUpper()))
                    .OrderBy(t => t.Nome).ToPagedList(numeroPagina, tamanhoPagina);

            }
            else
            {
                cliente = db.Clientes.OrderBy(c => c.Nome).ToPagedList(numeroPagina, tamanhoPagina);
            }
            return View("Index", cliente);
        }

        // GET: Cliente
        public ActionResult Index(string ordenacao, int? pagina)
        {
            ViewBag.OrdenacaoAtual = ordenacao;
            ViewBag.NomeParam = string.IsNullOrEmpty(ordenacao) ? "Nome_desc" : "";
            ViewBag.EmailParam = ordenacao == "Email" ? "Email_desc" : "Email";

            var clientes = from c in db.Clientes select c;

            int tamanhoPagina = 5;
            int numeroPagina = pagina ?? 1;

            switch (ordenacao)
            {
                case "Nome_desc":
                    clientes = clientes.OrderByDescending(s => s.Nome);
                    break;
                case "Email":
                    clientes = clientes.OrderBy(s => s.Email);
                    break;
                case "Email_desc":
                    clientes = clientes.OrderByDescending(s => s.Email);
                    break;
                default:
                    clientes = clientes.OrderBy(s => s.Nome);
                    break;
            }

            return View(clientes.ToPagedList(numeroPagina, tamanhoPagina));
        }
        
        // GET: Cliente/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClienteID,Nome,Endereco,Telefone,CEP,Email")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Clientes.Add(cliente);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cliente);
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClienteID,Nome,Endereco,Telefone,CEP,Email")] Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return HttpNotFound();
            }
            return View(cliente);
        }

        // POST: Cliente/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            db.Clientes.Remove(cliente);
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
