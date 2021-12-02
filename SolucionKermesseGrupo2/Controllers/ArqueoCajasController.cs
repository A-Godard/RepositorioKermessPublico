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
    public class ArqueoCajasController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: ArqueoCajas

        public ActionResult verReporte(string tipo)
        {
            LocalReport rpt = new LocalReport();
            string mt, enc, f;
            string[] s;
            Warning[] w;


            string ruta = Path.Combine(Server.MapPath("~/Reportes"), "RptArqueoCaja.rdlc");
            rpt.ReportPath = ruta;

            BDKermesseEntities modelo = new BDKermesseEntities();
            List<VwArqueoCaja> ls = new List<VwArqueoCaja>();
            ls = modelo.VwArqueoCaja.ToList();


            ReportDataSource rds = new ReportDataSource("DsArqueoCaja", ls);
            rpt.DataSources.Add(rds);

            byte[] b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);

            return File(b, mt);
        }

        public ActionResult Index()
        {
            var arqueoCaja = db.ArqueoCaja.Include(a => a.Kermesse1).Include(a => a.Usuario).Include(a => a.Usuario1).Include(a => a.Usuario2);
            return View(arqueoCaja.ToList());
        }

        // GET: ArqueoCajas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCaja arqueoCaja = db.ArqueoCaja.Find(id);
            if (arqueoCaja == null)
            {
                return HttpNotFound();
            }
            return View(arqueoCaja);
        }

        public ActionResult Crear()
        {

            ViewBag.Kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Crear(ArqueoCaja arqueo)
        {
            if (ModelState.IsValid)
            {

                ArqueoCaja a = new ArqueoCaja();
                a.kermesse = arqueo.kermesse;
                a.fechaArqueo = arqueo.fechaArqueo;
                a.granTotal = arqueo.granTotal;
                a.usuarioCreacion = 1;
                a.fechaCreacion = DateTime.Now;

                db.ArqueoCaja.Add(a);
                db.SaveChanges();
                ModelState.Clear();

            }
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");



            return RedirectToAction("Index");
        }

        /*// GET: ArqueoCajas/Create
        public ActionResult Create()
        {
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName");
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName");
            return View();
        }

        // POST: ArqueoCajas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "idArqueoCaja,kermesse,fechaArqueo,granTotal,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] ArqueoCaja arqueoCaja)
        {
            if (ModelState.IsValid)
            {
                db.ArqueoCaja.Add(arqueoCaja);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", arqueoCaja.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioEliminacion);
            return View(arqueoCaja);
        }

        // GET: ArqueoCajas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCaja arqueoCaja = db.ArqueoCaja.Find(id);
            if (arqueoCaja == null)
            {
                return HttpNotFound();
            }
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", arqueoCaja.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioEliminacion);
            return View(arqueoCaja);
        }

        // POST: ArqueoCajas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "idArqueoCaja,kermesse,fechaArqueo,granTotal,usuarioCreacion,fechaCreacion,usuarioModificacion,fechaModificacion,usuarioEliminacion,fechaEliminacion")] ArqueoCaja arqueoCaja)
        {
            if (ModelState.IsValid)
            {
                db.Entry(arqueoCaja).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre", arqueoCaja.kermesse);
            ViewBag.usuarioCreacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioCreacion);
            ViewBag.usuarioModificacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioModificacion);
            ViewBag.usuarioEliminacion = new SelectList(db.Usuario, "idUsuario", "userName", arqueoCaja.usuarioEliminacion);
            return View(arqueoCaja);
        }

        // GET: ArqueoCajas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ArqueoCaja arqueoCaja = db.ArqueoCaja.Find(id);
            if (arqueoCaja == null)
            {
                return HttpNotFound();
            }
            return View(arqueoCaja);
        }

        // POST: ArqueoCajas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ArqueoCaja arqueoCaja = db.ArqueoCaja.Find(id);
            db.ArqueoCaja.Remove(arqueoCaja);
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
        }*/
    }
}
