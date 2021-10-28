using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using SolucionKermesseGrupo2.Models;

namespace SolucionKermesseGrupo2.Controllers
{
    public class OpcionsController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: Opcions
        public ActionResult Index()
        {
            return View(db.Opcion.ToList());
        }

        // GET: Opcions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opcion opcion = db.Opcion.Find(id);
            if (opcion == null)
            {
                return HttpNotFound();
            }
            return View(opcion);
        }

        // GET: Opcions/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Opcions/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idOpcion,opcionDescripcion,estado")] Opcion opcion)
        {
            if (ModelState.IsValid)
            {
                db.Opcion.Add(opcion);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(opcion);
        }

        // GET: Opcions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opcion opcion = db.Opcion.Find(id);
            if (opcion == null)
            {
                return HttpNotFound();
            }
            return View(opcion);
        }

        // POST: Opcions/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idOpcion,opcionDescripcion,estado")] Opcion opcion)
        {
            if (ModelState.IsValid)
            {
                db.Entry(opcion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(opcion);
        }

        // GET: Opcions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Opcion opcion = db.Opcion.Find(id);
            if (opcion == null)
            {
                return HttpNotFound();
            }
            return View(opcion);
        }

        // POST: Opcions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Opcion opcion = db.Opcion.Find(id);
            db.Opcion.Remove(opcion);
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

        public ActionResult VerReporte(string tipo)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptOpcion.rdlc");
            rpt.ReportPath = ruta;

            BDKermesseEntities modelo = new BDKermesseEntities();
            List<Opcion> ls = new List<Opcion>();
            ls = modelo.Opcion.ToList();

            ReportDataSource rd = new ReportDataSource("DsOpcion", ls);
            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);
        }
    }
}
