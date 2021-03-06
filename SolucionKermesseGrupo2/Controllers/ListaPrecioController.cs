using Microsoft.Reporting.WebForms;
using SolucionKermesseGrupo2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SolucionKermesseGrupo2.Controllers
{
    [Authorize]
    public class ListaPrecioController : Controller
    {

        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: ListaPrecio
        public ActionResult Index(string ValorBusqued)
        {
            //var listas = from m in db.VwListaPrecio
            var listas=1;
                          

            if (!String.IsNullOrEmpty(ValorBusqued))
            {

                
            }

            return View(listas);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //VwListaPrecio lista = db.VwListaPrecio.Find(id);
            ViewBag.CatProd = new SelectList(db.CategoriaProducto, "idCatProd", "nombre");
            return View();

        }

        public ActionResult Crear()
        {
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Crear(ListaPrecio listaPrecio)
        {
            if (ModelState.IsValid)
            {
                ListaPrecio l = new ListaPrecio();
                l.kermesse = listaPrecio.kermesse;
                l.nombre = listaPrecio.nombre;
                l.descripcion = listaPrecio.descripcion;

                db.ListaPrecio.Add(l);
                db.SaveChanges();
                ModelState.Clear();
            }
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");

            return View("Crear");
        }

        public ActionResult VerReporte(string tipo)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptListaPrecio.rdlc");
            rpt.ReportPath = ruta;

            BDKermesseEntities modelo = new BDKermesseEntities();
            /*List<VwListaPrecio> ls = new List<VwListaPrecio>();
            ls = modelo.VwListaPrecio.ToList();*/

            ReportDataSource rd = new ReportDataSource("DSListaPrecio");
            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);
        }

        public ActionResult VerRptFiltradoListaPrecio(VwListaPrecio Lp)
        {
            LocalReport lrpt = new LocalReport();

            string mt, enc, f, tipo;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "rptVwListaPrecio.rdlc");
            lrpt.ReportPath = ruta;
            
            var listaPrecio = db.VwListaPrecio.Where(x => x.idKermesse == Lp.idKermesse  && x.idCatProd == Lp.idCatProd  );

            ReportDataSource rds = new ReportDataSource("DsListaPrecio", listaPrecio);
            lrpt.DataSources.Add(rds);
            tipo = "PDF";

            var b = lrpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);

        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecio lista = db.ListaPrecio.Find(id);
            if (lista == null)
            {
                return HttpNotFound();
            }
            return View(lista);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idListaPrecio, kermesse, nombre, descripcion, estado")] ListaPrecio lista)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lista).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(lista);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecio lista = db.ListaPrecio.Find(id);
            if (lista == null)
            {
                return HttpNotFound();
            }
            return View(lista);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListaPrecio lista = db.ListaPrecio.Find(id);
            db.ListaPrecio.Remove(lista);
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