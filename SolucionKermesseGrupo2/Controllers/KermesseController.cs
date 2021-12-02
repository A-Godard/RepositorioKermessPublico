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
    public class KermesseController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: Kermesse
        public ActionResult Index(string ValorBusqued)
        {
            var kermesse = from m in db.VwKermesse
                            select m;

            if (!String.IsNullOrEmpty(ValorBusqued))
            {

                kermesse = kermesse.Where(s => s.Kermesse.Contains(ValorBusqued));
            }

            return View(kermesse.ToList());
        }

        public ActionResult ReporteKermesse(string ValorBusqued)
        {
            var kermesse = from m in db.VwIngresoKermesse
                           select m;

            if (!String.IsNullOrEmpty(ValorBusqued))
            {

                kermesse = kermesse.Where(s => s.NombreKermesse.Contains(ValorBusqued));
            }

            return View(kermesse.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VwKermesse kermesse = db.VwKermesse.Find(id);
            if (kermesse == null)
            {
                return HttpNotFound();
            }
            return View(kermesse);
        }

        public ActionResult Crear()
        {
            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre");
            ViewBag.usuario = new SelectList(db.Usuario, "idUsuario", "userName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Crear(Kermesse kermesse)
        {
            if (ModelState.IsValid)
            {
                Kermesse k = new Kermesse();
                k.parroquia = kermesse.parroquia;
                k.nombre = kermesse.nombre;
                k.fInicio = kermesse.fInicio;
                k.fFinal = kermesse.fFinal;
                k.descripcion = kermesse.descripcion;
                k.usuarioCreacion = -1;
                k.fechaCreacion = DateTime.Now;

                db.Kermesse.Add(k);
                db.SaveChanges();
                ModelState.Clear();

            }

            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre");
            ViewBag.usuario = new SelectList(db.Usuario, "idUsuario", "userName");

            return View("Crear");
        }

        public ActionResult VerReporte(string tipo)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptKermesse.rdlc");
            rpt.ReportPath = ruta;

            BDKermesseEntities modelo = new BDKermesseEntities();
            List<VwKermesse> ls = new List<VwKermesse>();
            ls = modelo.VwKermesse.ToList();

            ReportDataSource rd = new ReportDataSource("DSKermesse", ls);
            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);
        }


        public ActionResult VerReporteProducto(int id)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            var kermesses = from m in db.Kermesse select m;
            if (id != null)
            {
                Kermesse kermesse = db.Kermesse.Find(id);

            }

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptKermesse2.rdlc");
            rpt.ReportPath = ruta;

            BDKermesseEntities modelo = new BDKermesseEntities();
            List<Kermesse> ls = new List<Kermesse>();
            ls = modelo.Kermesse.ToList();


            ReportDataSource rds = new ReportDataSource("DSKermesse", ls);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render("PDF", null, out mt, out enc, out f, out s, out w);


            return File(b, mt);
        }

        public ActionResult VerReporte2(string tipo, string valorB, string opcR)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptKermesse2.rdlc");
            rpt.ReportPath = ruta;

            var listaP = from VwKermesse in db.VwKermesse select VwKermesse;

            if (!string.IsNullOrEmpty(valorB) && opcR.Equals("a"))
            {
                listaP = listaP.Where(VwKermesse => VwKermesse.Kermesse.Contains(valorB));
            }
            if (opcR.Equals("b"))
            {
                listaP = listaP.Where(VwKermesse => VwKermesse.idKermesse.ToString().Equals(valorB));
            }
            List<VwKermesse> ls = new List<VwKermesse>();
            ls = listaP.ToList();

            ReportDataSource rd = new ReportDataSource("DSKermesse", ls);
            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kermesse kermesse = db.Kermesse.Find(id);
            if (kermesse == null)
            {
                return HttpNotFound();
            }
            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre");
            ViewBag.usuario = new SelectList(db.Usuario, "idUsuario", "userName");

            return View(kermesse);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idKermesse, parroquia, nombre, fInicio, fFinal, descripcion, estado, usuarioCreacion, fechaCreacion, usuarioModificacion, fechaModificacion, usuarioEliminacion, fechaEliminacion")] Kermesse kermesse)
        {
            if (ModelState.IsValid)
            {
                db.Entry(kermesse).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre");
            ViewBag.usuario = new SelectList(db.Usuario, "idUsuario", "userName");

            return View(kermesse);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kermesse kermesse = db.Kermesse.Find(id);
            if (kermesse == null)
            {
                return HttpNotFound();
            }
            return View(kermesse);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Kermesse kermesse = db.Kermesse.Find(id);
            db.Kermesse.Remove(kermesse);
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