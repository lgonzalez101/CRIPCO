using CRIPCO.BD;
using CRIPCO.Models;
using CRIPCO.Models.Base;
using CRIPCO.Models.Expedientes;
using CRIPCO.Models.Usuario;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
                    Paciente = x.Cita.Persona.Nombre +" "+ x.Cita.Persona.Apellido,
                    Extension = x.ExtensionDocumento
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

        public FileResult DescargarArchivo(int Id)
        {
            using (var context = new CripcoEntities())
            {
                var expediente = context.Expediente.Find(Id);

                byte[] fileBytes = expediente.Documento;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, expediente.ExtensionDocumento);
                 
            }
        }

        [HttpPost]
        public ActionResult CrearExpediente(ExpedientesViewModel model)
        {
            using (var context = new CripcoEntities())
            {
                byte[] uploadedFile = new byte[model.Documento.InputStream.Length];
                
                context.Expediente.Add(new Expediente {
                    CitaID = model.CitaID,
                    Activo = true,
                    ExtensionDocumento = model.Documento.FileName,
                    Comentario = model.Comentario,
                    Documento = uploadedFile,
                    CreadoPor = User.Identity.Name,
                    ModificadoPor = User.Identity.Name,
                    FechaCreado = DateTime.Now,
                });
                var result = context.SaveChanges() > 0;
               return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public ActionResult MostrarEditarExpediente(int id)
        {
            using (var context = new CripcoEntities())
            {
                var expediente = context.Expediente.Find(id);
                return PartialView(new ExpedientesViewModel
                {
                    Comentario = expediente.Comentario,
                    Documento = (HttpPostedFileBase)new MemoryPostedFile(expediente.Documento),
                    Activo = expediente.Activo,
                    Id = expediente.ExpedienteID
            });
            }
        }
        
        [HttpPost]
        public ActionResult EditarExpediente(ExpedientesViewModel model)
        {
            using (var context = new CripcoEntities())
            {
                var expediente = context.Expediente.Find(model.Id);
                expediente.Comentario = model.Comentario;
                expediente.ModificadoPor = User.Identity.Name;
                context.Entry(expediente).State = EntityState.Modified;
                var result = context.SaveChanges() > 0;
                return Json(new MensajeRespuestaViewModel
                {
                    Titulo = "Editar Expediente",
                    Mensaje = result ? "Se edito satisfactoriamente" : "Error al editar el expediente",
                    Estado = result
                }, JsonRequestBehavior.AllowGet);
            }
        }



    //// GET: Expedientes/Details/5
    //public ActionResult Details(int id)
    //    {
    //        return View();
    //    }

    //    // GET: Expedientes/Create
    //    public ActionResult Create()
    //    {
    //        return View();
    //    }

    //    // POST: Expedientes/Create
    //    [HttpPost]
    //    public ActionResult Create(FormCollection collection)
    //    {
    //        try
    //        {
    //            // TODO: Add insert logic here

    //            return RedirectToAction("Index");
    //        }
    //        catch
    //        {
    //            return View();
    //        }
    //    }

       

    //    // GET: Expedientes/Delete/5
    //    public ActionResult Delete(int id)
    //    {
    //        return View();
    //    }

    //    // POST: Expedientes/Delete/5
    //    [HttpPost]
    //    public ActionResult Delete(int id, FormCollection collection)
    //    {
    //        try
    //        {
    //            // TODO: Add delete logic here

    //            return RedirectToAction("Index");
    //        }
    //        catch
    //        {
    //            return View();
    //        }
    //    }
    }

    public class MemoryPostedFile : HttpPostedFileBase
    {
        private readonly byte[] fileBytes;

        public MemoryPostedFile(byte[] fileBytes, string fileName = null)
        {
            this.fileBytes = fileBytes;
            this.FileName = fileName;
            this.InputStream = new MemoryStream(fileBytes);
        }

        public override int ContentLength => fileBytes.Length;

        public override string FileName { get; }

        public override Stream InputStream { get; }
    }


}
