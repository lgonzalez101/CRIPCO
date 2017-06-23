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
    public class CitaController : Controller
    {
        private CripcoEntities db = new CripcoEntities();

        // GET: Cita
        public ActionResult Index()
        {
            var cita = db.Cita.Include(c => c.Horario).Include(c => c.Persona);
            return View(cita.ToList());
        }

        // GET: Cita/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cita cita = db.Cita.Find(id);
            if (cita == null)
            {
                return HttpNotFound();
            }
            return View(cita);
        }

        // GET: Cita/Create
        public ActionResult Create()
        {
            ViewBag.HorarioID = new SelectList(db.Horario, "HorarioID", "CreadoPor");
            ViewBag.PersonaPacienteID = new SelectList(db.Persona, "PersonaID", "IdAspnetUser");
            return View();
        }

        // POST: Cita/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CitaID,HorarioID,PersonaPacienteID,CreadoPor,FechaCreado,ModificadoPor,Activo")] Cita cita)
        {
            if (ModelState.IsValid)
            {
                db.Cita.Add(cita);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.HorarioID = new SelectList(db.Horario, "HorarioID", "CreadoPor", cita.HorarioID);
            ViewBag.PersonaPacienteID = new SelectList(db.Persona, "PersonaID", "IdAspnetUser", cita.PersonaPacienteID);
            return View(cita);
        }

        // GET: Cita/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cita cita = db.Cita.Find(id);
            if (cita == null)
            {
                return HttpNotFound();
            }
            ViewBag.HorarioID = new SelectList(db.Horario, "HorarioID", "CreadoPor", cita.HorarioID);
            ViewBag.PersonaPacienteID = new SelectList(db.Persona, "PersonaID", "IdAspnetUser", cita.PersonaPacienteID);
            return View(cita);
        }

        // POST: Cita/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CitaID,HorarioID,PersonaPacienteID,CreadoPor,FechaCreado,ModificadoPor,Activo")] Cita cita)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cita).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HorarioID = new SelectList(db.Horario, "HorarioID", "CreadoPor", cita.HorarioID);
            ViewBag.PersonaPacienteID = new SelectList(db.Persona, "PersonaID", "IdAspnetUser", cita.PersonaPacienteID);
            return View(cita);
        }

        // GET: Cita/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Cita cita = db.Cita.Find(id);
            if (cita == null)
            {
                return HttpNotFound();
            }
            return View(cita);
        }

        // POST: Cita/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Cita cita = db.Cita.Find(id);
            db.Cita.Remove(cita);
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
