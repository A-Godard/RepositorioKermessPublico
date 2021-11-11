﻿using Microsoft.Reporting.WebForms;
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
            ViewBag.catProducto = new SelectList(db.CategoriaProducto, "idCatProd", "nombre");
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

                ViewBag.comunidad = new SelectList(db.Comunidad, "idComunidad", "nombre");
                ViewBag.catProducto = new SelectList(db.CategoriaProducto, "idCatProd", "nombre");
            }

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

    }
}