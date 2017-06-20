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
    [Authorize(Roles = "Administrador")]
    public class AreaController : Controller
    {
        private CripcoEntities db = new CripcoEntities();

        // GET: Area
        public ActionResult Index()
        {
            return View(db.Area.ToList());
        }

        // GET: Area/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Area.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // GET: Area/Create
        public ActionResult Create()
        {
            return PartialView();
        }

        // POST: Area/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AreaID,Nombre,CreadoPor,FechaCreado,ModificadoPor,Activo")] Area area)
        {
            if (ModelState.IsValid)
            {
                area.FechaCreado = DateTime.Now;
                area.CreadoPor = User.Identity.Name;
                area.ModificadoPor = User.Identity.Name;
                area.Activo = true;
                db.Area.Add(area);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(area);
        }

        // GET: Area/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Area.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return PartialView(area);
        }

        // POST: Area/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AreaID,Nombre,CreadoPor,FechaCreado,ModificadoPor,Activo")] Area area)
        {
            if (ModelState.IsValid)
            {
                area.ModificadoPor = User.Identity.Name;
                db.Entry(area).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(area);
        }

        // GET: Area/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Area area = db.Area.Find(id);
            if (area == null)
            {
                return HttpNotFound();
            }
            return View(area);
        }

        // POST: Area/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Area area = db.Area.Find(id);
            db.Area.Remove(area);
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
