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
    public class ProductoController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();


        // GET: Producto
        public ActionResult Index(string ValorBusqued)
        {
            var productos = from m in db.VwProducto
                          select m;

            if (!String.IsNullOrEmpty(ValorBusqued))
            {

                productos = productos.Where(s => s.Producto.Contains(ValorBusqued));
            }

            return View(productos.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VwProducto producto = db.VwProducto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }


        public ActionResult Crear()
        {
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre" );
            ViewBag.catProd = new SelectList(db.CategoriaProducto, "idCatProd", "nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Crear(Producto producto)
        {
            if (ModelState.IsValid)
            {
                Producto p = new Producto();
                p.comunidad = producto.comunidad;
                p.catProd = producto.catProd;
                p.nombre = producto.nombre;
                p.descripcion = producto.descripcion;
                p.cantidad = producto.cantidad;
                p.precioVSugerido = producto.precioVSugerido;
                

                db.Producto.Add(p);
                db.SaveChanges();
                ModelState.Clear();

            }

            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre");
            ViewBag.catProd = new SelectList(db.CategoriaProducto, "idCatProd", "nombre");

            return View("Crear");
        }

        public ActionResult VerReporte(string tipo)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptProducto.rdlc");
            rpt.ReportPath = ruta;

            BDKermesseEntities modelo = new BDKermesseEntities();
            List<VwProducto> ls = new List<VwProducto>();
            ls = modelo.VwProducto.ToList();

            ReportDataSource rd = new ReportDataSource("DSProducto", ls);
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

            var productos = from m in db.Producto select m;
            if (id != null)
            {
                Producto producto = db.Producto.Find(id);

            }

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptProducto2.rdlc");
            rpt.ReportPath = ruta;

            BDKermesseEntities modelo = new BDKermesseEntities();
            List<Producto> ls = new List<Producto>();
            ls = modelo.Producto.ToList();


            ReportDataSource rds = new ReportDataSource("DSProducto", ls);
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

            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptProducto2.rdlc");
            rpt.ReportPath = ruta;

            var listaP = from VwProducto in db.VwProducto select VwProducto;

            if(!string.IsNullOrEmpty(valorB) && opcR.Equals("a"))
            {
                listaP = listaP.Where(VwProducto => VwProducto.Producto.Contains(valorB));
            }
            if(opcR.Equals("b"))
            {
                listaP = listaP.Where(VwProducto => VwProducto.idProducto.ToString().Equals(valorB));
            }
            List<VwProducto> ls = new List<VwProducto>();
            ls = listaP.ToList();

            ReportDataSource rd = new ReportDataSource("DSProducto", ls);
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
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre");
            ViewBag.catProd = new SelectList(db.CategoriaProducto, "idCatProd", "nombre");
            
            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idProducto, comunidad, catProd, nombre, descripcion, cantidad, precioVSugerido, estado")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(producto).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre");
            ViewBag.catProd = new SelectList(db.CategoriaProducto, "idCatProd", "nombre");
            return View(producto);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Producto producto = db.Producto.Find(id);
            if (producto == null)
            {
                return HttpNotFound();
            }
            return View(producto);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Producto producto = db.Producto.Find(id);
            db.Producto.Remove(producto);
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