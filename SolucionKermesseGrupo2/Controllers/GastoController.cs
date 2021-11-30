using SolucionKermesseGrupo2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SolucionKermesseGrupo2.Controllers
{
    public class GastoController : Controller
    {
        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: Gasto
        public ActionResult Index(string dato)
        {
            var gastos = from g in db.VwGasto
                         select g;
            if (!string.IsNullOrEmpty(dato))
            {
                gastos = gastos.Where(g => g.Kermesse.Contains(dato) && g.Categoria.Contains(dato));                
            }
            return View(gastos.ToList());
        }
        public ActionResult Detalle(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VwGasto gasto = db.VwGasto.Find(id);
            if (gasto == null)
            {
                return HttpNotFound();
            }
            return View(gasto);
        }

        public ActionResult Crear()
        {

            ViewBag.Kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");

            ViewBag.CategoriaGasto = new SelectList(db.CategoriaGasto, "idCatGasto", "nombreCategoria");



            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Crear(Gasto gasto)
        {
            if(ModelState.IsValid)
            {

                Gasto g = new Gasto();
                g.kermesse = gasto.kermesse;
                g.catGasto = gasto.catGasto;
                g.concepto = gasto.concepto;
                g.monto = gasto.monto;
                g.fechGasto = gasto.fechGasto;
                g.usuarioCreacion = 1;
                g.fechaCreacion = DateTime.Now;

                db.Gasto.Add(g);
                db.SaveChanges();
                ModelState.Clear();

            }
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");

            ViewBag.catGasto = new SelectList(db.CategoriaGasto, "idCatGasto", "nombreCategoria");



            return View("Crear");
        }



    }

}