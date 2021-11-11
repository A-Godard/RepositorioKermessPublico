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
    public class ListaPrecioController : Controller
    {

        private BDKermesseEntities db = new BDKermesseEntities();

        // GET: ListaPrecio
        public ActionResult Index(string ValorBusqued)
        {
            var listas = from m in db.VwListaPrecio
                            select m;

            if (!String.IsNullOrEmpty(ValorBusqued))
            {

                listas = listas.Where(s => s.Lista.Contains(ValorBusqued));
            }

            return View(listas.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VwListaPrecio lista = db.VwListaPrecio.Find(id);
            if (lista == null)
            {
                return HttpNotFound();
            }
            return View(lista);
        }

        public ActionResult Crear()
        {
            ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");
            ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre");
            ViewBag.usuario = new SelectList(db.Usuario, "idUsuario", "nombres" + "apellidos");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Crear(ListaPrecio listaPrecio, ListaPrecioDet listaPrecioDet, Kermesse kermesse, Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                ListaPrecio l = new ListaPrecio();
                ListaPrecioDet ld = new ListaPrecioDet();
                Kermesse k = new Kermesse();
                Usuario u = new Usuario();
                l.kermesse = listaPrecio.kermesse;
                k.parroquia = kermesse.parroquia;
                u.nombres = usuario.nombres;
                ld.listaPrecio = listaPrecioDet.listaPrecio;
                l.nombre = listaPrecio.nombre;
                l.descripcion = listaPrecio.descripcion;
                


                db.ListaPrecio.Add(l);
                db.SaveChanges();
                ModelState.Clear();

                ViewBag.kermesse = new SelectList(db.Kermesse, "idKermesse", "nombre");
                ViewBag.parroquia = new SelectList(db.Parroquia, "idParroquia", "nombre");
                ViewBag.usuario = new SelectList(db.Usuario, "idUsuario", "nombres" + "apellidos");
            }

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
            List<VwListaPrecio> ls = new List<VwListaPrecio>();
            ls = modelo.VwListaPrecio.ToList();

            ReportDataSource rd = new ReportDataSource("DSListaPrecio", ls);
            rpt.DataSources.Add(rd);

            var b = rpt.Render(tipo, null, out mt, out enc, out f, out s, out w);
            return new FileContentResult(b, mt);
        }
    }
}