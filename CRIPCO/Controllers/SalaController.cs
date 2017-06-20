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
    public class SalaController : Controller
    {
        private CripcoEntities db = new CripcoEntities();

        // GET: Sala
        public ActionResult Index()
        {
            var sala = db.Sala.Include(s => s.Area);
            return View(sala.ToList());
        }

        // GET: Sala/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sala sala = db.Sala.Find(id);
            if (sala == null)
            {
                return HttpNotFound();
            }
            return View(sala);
        }

        // GET: Sala/Create
        public ActionResult Create()
        {
            ViewBag.AreaID = new SelectList(db.Area, "AreaID", "Nombre");
            return PartialView();
        }

        // POST: Sala/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SalaID,AreaID,Nombre,Tipo,CreadoPor,FechaCreado,ModificadoPor,Activo")] Sala sala)
        {
            if (ModelState.IsValid)
            {
                sala.Activo = true;
                sala.CreadoPor = User.Identity.Name;
                sala.FechaCreado = DateTime.Now;
                sala.ModificadoPor = User.Identity.Name;
                db.Sala.Add(sala);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AreaID = new SelectList(db.Area, "AreaID", "Nombre", sala.AreaID);
            return View(sala);
        }

        // GET: Sala/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sala sala = db.Sala.Find(id);
            if (sala == null)
            {
                return HttpNotFound();
            }
            ViewBag.AreaID = new SelectList(db.Area, "AreaID", "Nombre", sala.AreaID);
            return PartialView(sala);
        }

        // POST: Sala/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SalaID,AreaID,Nombre,Tipo,CreadoPor,FechaCreado,ModificadoPor,Activo")] Sala sala)
        {
            if (ModelState.IsValid)
            {
                sala.ModificadoPor = User.Identity.Name;
                db.Entry(sala).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AreaID = new SelectList(db.Area, "AreaID", "Nombre", sala.AreaID);
            return View(sala);
        }

        // GET: Sala/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sala sala = db.Sala.Find(id);
            if (sala == null)
            {
                return HttpNotFound();
            }
            return View(sala);
        }

        // POST: Sala/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Sala sala = db.Sala.Find(id);
            db.Sala.Remove(sala);
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
