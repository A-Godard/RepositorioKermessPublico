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
    [Authorize]
    public class CategoriaGastoesController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: CategoriaGastoes
        public ActionResult Index(string ValorBusqued)
        {
            var CatGasto = from m in db.CategoriaGasto select m;
            if (!String.IsNullOrEmpty(ValorBusqued))
            {
                CatGasto = CatGasto.Where(s => s.nombreCategoria.Contains(ValorBusqued));
            }
            return View(CatGasto.ToList());
        }

      

        //Metodo del reporte categoria gastos GET: VerReporte

        public ActionResult VerReporte(string tipo)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptCategoriaGasto.rdlc");
            rpt.ReportPath = ruta;

            BDKermesseEntities modelo = new BDKermesseEntities();
            List<CategoriaGasto> ls = new List<CategoriaGasto>();
            ls = modelo.CategoriaGasto.ToList();

            ReportDataSource rds = new ReportDataSource("DsCategoriaGasto",ls);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }






        // GET: CategoriaGastoes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaGasto categoriaGasto = db.CategoriaGasto.Find(id);
            if (categoriaGasto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaGasto);
        }

        // GET: CategoriaGastoes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CategoriaGastoes/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idCatGasto,nombreCategoria,descripcion,estado")] CategoriaGasto categoriaGasto)
        {
            if (ModelState.IsValid)
            {
                db.CategoriaGasto.Add(categoriaGasto);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(categoriaGasto);
        }

        // GET: CategoriaGastoes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaGasto categoriaGasto = db.CategoriaGasto.Find(id);
            if (categoriaGasto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaGasto);
        }

        // POST: CategoriaGastoes/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idCatGasto,nombreCategoria,descripcion,estado")] CategoriaGasto categoriaGasto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoriaGasto).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(categoriaGasto);
        }

        // GET: CategoriaGastoes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoriaGasto categoriaGasto = db.CategoriaGasto.Find(id);
            if (categoriaGasto == null)
            {
                return HttpNotFound();
            }
            return View(categoriaGasto);
        }

        // POST: CategoriaGastoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CategoriaGasto categoriaGasto = db.CategoriaGasto.Find(id);
            db.CategoriaGasto.Remove(categoriaGasto);
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
