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
     [Authorize]
    public class CitaController : BaseController
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

        [HttpGet]
        public ActionResult CrearCita()
        {
            ViewBag.ListaUsuarios = new SelectList(db.Persona.Where(x => x.Activo && x.AspNetUsers.AspNetRoles.Any(y=>y.Name=="Paciente")), "PersonaID", "Nombre");
            ViewBag.ListaEspecialidades = new SelectList(db.Especialidad.Where(x => x.Activo), "EspecialidadID", "Nombre");
            return View();
        }

        [HttpGet]
        public ActionResult CargaHorariosDisponibles(DateTime Fecha, int IdEspecialidad)
        {
            var list = new List<Horario>();
            Fecha=Fecha.AddHours(DateTime.Now.Hour);
         //   if(Fecha<DateTime.Now) return PartialView(list);
            foreach (var horario in db.Horario.Where(x => x.Activo && x.Reservado == false && x.Persona.PersonaEspecialidad.Any(y => y.EspecialidadID == IdEspecialidad)).ToList())
            {
                if (horario.Hora > DateTime.Now && horario.Hora.Date == Fecha.Date)
                {
                    list.Add(horario);
                }

            }

            return PartialView(list);
          //  return PartialView(db.Horario.Where(x=> DateTime.Compare(Fecha.Date, x.Hora.Date)==0 && x.Persona.PersonaEspecialidad.Any(y=>y.EspecialidadID==IdEspecialidad) && x.Activo && x.Reservado==false).ToList());
        }

        [HttpPost]
        public ActionResult ReservarCita(int IdHorario, int? IdUsuario)
        {
            using (var context = new CripcoEntities())
            {
                context.Cita.Add(new Cita {
                    Activo=true,
                    CreadoPor= User.Identity.Name,
                    ModificadoPor = User.Identity.Name,
                    FechaCreado = DateTime.Now,
                    HorarioID = IdHorario,
                    PersonaPacienteID = IdUsuario.HasValue?IdUsuario??0: ObtenerIdUsuario(), 
                });

                var horario = context.Horario.Find(IdHorario);
                horario.Reservado = true;

                var result= context.SaveChanges()>0;

                return Json(EnviarResultado(result, "Reservar Cita"), JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult HistorialCitasPaciente()
        {
                var idusuario = ObtenerIdUsuario();
                var lista = db.Cita.Where(x => x.PersonaPacienteID == idusuario).ToList();
                return View(lista);

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
            ViewBag.PersonaPacienteID = new SelectList(db.Persona, "PersonaID", "Nombre", cita.PersonaPacienteID);
            return PartialView(cita);
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
                var citamodel = db.Cita.Find(cita.CitaID);
                citamodel.Activo = cita.Activo;
               // citamodel.PersonaPacienteID = cita.PersonaPacienteID;
                db.Entry(citamodel).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.HorarioID = new SelectList(db.Horario, "HorarioID", "CreadoPor", cita.HorarioID);
            ViewBag.PersonaPacienteID = new SelectList(db.Persona, "PersonaID", "Nombre", cita.PersonaPacienteID);
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
