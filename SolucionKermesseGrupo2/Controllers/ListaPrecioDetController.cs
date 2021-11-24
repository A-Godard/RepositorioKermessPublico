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
    public class ListaPrecioDetController : Controller
    {

        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: ListaPrecioDet
        public ActionResult Index(string ValorBusqued)
        {
            var listaDet = from m in db.VwListaPrecioDet
                         select m;

            if (!String.IsNullOrEmpty(ValorBusqued))
            {

                listaDet = listaDet.Where(s => s.Lista.Contains(ValorBusqued));
            }

            return View(listaDet.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VwListaPrecioDet listaDet = db.VwListaPrecioDet.Find(id);
            if (listaDet == null)
            {
                return HttpNotFound();
            }
            return View(listaDet);
        }

        public ActionResult Crear()
        {
            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre");
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Crear(ListaPrecioDet listaPrecioDet)
        {
            if (ModelState.IsValid)
            {
                ListaPrecioDet ld = new ListaPrecioDet();
                ld.listaPrecio = listaPrecioDet.listaPrecio;
                ld.producto = listaPrecioDet.producto;
                ld.precioVenta = listaPrecioDet.precioVenta;


                db.ListaPrecioDet.Add(ld);
                db.SaveChanges();
                ModelState.Clear();
            }
            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre");
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre");

            return View("Crear");
        }

        public ActionResult VerReporte(string tipo)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptListaPrecioDet.rdlc");
            rpt.ReportPath = ruta;

            BDKermesseEntities modelo = new BDKermesseEntities();
            List<VwListaPrecioDet> ls = new List<VwListaPrecioDet>();
            ls = modelo.VwListaPrecioDet.ToList();

            ReportDataSource rd = new ReportDataSource("DSListaPrecioDet", ls);
            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);
        }

        public ActionResult VerReporteListaDet1(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var listaPrecioDet = from m in db.ListaPrecioDet select m;
            if (id != null)
            {
                ListaPrecioDet listaDet = db.ListaPrecioDet.Find(id);

            }

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptListaPrecioDet1.rdlc");
            rpt.ReportPath = ruta;

            BDKermesseEntities modelo = new BDKermesseEntities();
            List<ListaPrecioDet> ls = new List<ListaPrecioDet>();
            ls = modelo.ListaPrecioDet.ToList();


            ReportDataSource rds = new ReportDataSource("DSListaPrecioDet", ls);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", null, out mt, out enc, out f, out s, out w);


            return File(b, mt);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecioDet listaDet = db.ListaPrecioDet.Find(id);
            if (listaDet == null)
            {
                return HttpNotFound();
            }
            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre");
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre");

            return View(listaDet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idListaPrecioDet, listaPrecio, producto, precioVenta")] ListaPrecioDet listaDet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(listaDet).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.listaPrecio = new SelectList(db.ListaPrecio, "idListaPrecio", "nombre");
            ViewBag.producto = new SelectList(db.Producto, "idProducto", "nombre");

            return View(listaDet);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ListaPrecioDet listaDet = db.ListaPrecioDet.Find(id);
            if (listaDet == null)
            {
                return HttpNotFound();
            }
            return View(listaDet);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ListaPrecioDet listaDet = db.ListaPrecioDet.Find(id);
            db.ListaPrecioDet.Remove(listaDet);
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