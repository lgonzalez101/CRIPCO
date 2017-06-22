using CRIPCO.BD;
using CRIPCO.Models.Expedientes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRIPCO.Controllers
{
    public class ExpedientesController : BaseController
    {
        // GET: Expedientes
        public ActionResult Index()
        {
            using (var context = new CripcoEntities())
            {
                var listaExpediente = context.Expediente.Select(x=>new ListaExpedienteViewModel {
                    Id=x.ExpedienteID,
                    Comentario = x.Comentario,
                    Documento = x.Documento,
                    Activo = x.Activo,
                    Paciente = x.Cita.Persona.Nombre +" "+ x.Cita.Persona.Apellido

                }).ToList();
                return View(listaExpediente);
            }            
        }
        [HttpGet]
        public ActionResult MostrarCrearExpediente(int Id)
        {
            using (var context = new CripcoEntities())
            {
                var cita = context.Cita.FirstOrDefault(x => x.HorarioID == Id && x.Activo);
                return PartialView(new ExpedientesViewModel { CitaID = cita.CitaID});
            }
        }

        [HttpPost]
        public ActionResult CrearExpediente(ExpedientesViewModel model)
        {
            using (var context = new CripcoEntities())
            {
                byte[] uploadedFile = new byte[model.Documento2.InputStream.Length];
                model.Documento2.InputStream.Read(uploadedFile, 0, uploadedFile.Length);


                context.Expediente.Add(new Expediente {
                    CitaID = model.CitaID,
                    Activo = true,
                    Comentario = model.Comentario,
                    Documento = uploadedFile,
                    CreadoPor = User.Identity.Name,
                    ModificadoPor = User.Identity.Name,
                    FechaCreado = DateTime.Now,
                });
                var result = context.SaveChanges() > 0;
                // return Json((context.SaveChanges() > 0, "Crear Expediente"), JsonRequestBehavior.AllowGet);
               return RedirectToAction("Index");
            }
        }

        // GET: Expedientes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Expedientes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Expedientes/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Expedientes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Expedientes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Expedientes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Expedientes/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
