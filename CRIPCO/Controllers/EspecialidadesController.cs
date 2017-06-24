using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CRIPCO.BD;

namespace CRIPCO.Controllers
{
    public class EspecialidadesController : Controller
    {
        private CripcoEntities db = new CripcoEntities();

        // GET: Especialidades
        public ActionResult Index()
        {
            return View(db.Especialidad.ToList());
        }

        // GET: Especialidades/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidad especialidad = db.Especialidad.Find(id);
            if (especialidad == null)
            {
                return HttpNotFound();
            }
            return View(especialidad);
        }

        // GET: Especialidades/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Especialidades/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EspecialidadID,Nombre,CreadoPor,FechaCreado,ModificadoPor,Activo")] Especialidad especialidad)
        {
            using (var context = new CripcoEntities())
            {
                context.Especialidad.Add(new Especialidad
                {
                    Nombre = especialidad.Nombre,
                    CreadoPor = User.Identity.Name,
                    ModificadoPor = User.Identity.Name,
                    FechaCreado = DateTime.Now,
                    Activo = true
                });
                var result = context.SaveChanges() > 0;
                // return Json((context.SaveChanges() > 0, "Crear Expediente"), JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index");
            }
        }

        // GET: Especialidades/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidad especialidad = db.Especialidad.Find(id);
            if (especialidad == null)
            {
                return HttpNotFound();
            }
            return PartialView(especialidad);
        }

        // POST: Especialidades/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EspecialidadID,Nombre,CreadoPor,FechaCreado,ModificadoPor,Activo")] Especialidad especialidad)
        {
            using (var context = new CripcoEntities())
            {
                context.Especialidad.Add(new Especialidad
                {
                    Nombre = especialidad.Nombre,
                    CreadoPor = User.Identity.Name,
                    ModificadoPor = User.Identity.Name,
                    FechaCreado = DateTime.Now,
                    Activo = true
                });
                var result = context.SaveChanges() > 0;
                // return Json((context.SaveChanges() > 0, "Crear Expediente"), JsonRequestBehavior.AllowGet);
                return RedirectToAction("Index");
            }
        }

        // GET: Especialidades/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Especialidad especialidad = db.Especialidad.Find(id);
            if (especialidad == null)
            {
                return HttpNotFound();
            }
            return View(especialidad);
        }

        // POST: Especialidades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Especialidad especialidad = db.Especialidad.Find(id);
            db.Especialidad.Remove(especialidad);
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
