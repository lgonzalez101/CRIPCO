using CRIPCO.BD;
using CRIPCO.Models.Horario;
using CRIPCO.Models.Usuario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRIPCO.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class HorarioController : BaseController
    {
        // GET: Horario
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Listar()
        {
            using (var context = new CripcoEntities())
            {
                var idUsuario = ObtenerIdUsuario();
                var jsonResult = Json(context.Horario.Any(x => x.PersonaID == idUsuario) ? context.Horario.Where(x=>x.PersonaID== idUsuario)?.Select(x => new ListaHorarioViewModel { Id = x.HorarioID, Fecha = x.Hora, Estado = x.Activo, Reservado= x.Reservado }).OrderBy(x => x.Fecha).ToList(): new List<ListaHorarioViewModel>(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        [HttpGet]
        public ActionResult MostrarCrearHorario()
        {
            using (var context = new CripcoEntities())
            {
                ViewBag.ListaSalas = context.Sala.Where(x => x.Activo ?? false).Select(x => new SelectListItem { Value = x.SalaID.ToString(), Text = x.Area.Nombre + " / " + x.Nombre }).ToList();
                return PartialView(new CrearHorarioViewModel { FechaInicio = DateTime.Now, FechaFinal = DateTime.Now });
            }

        }

        [HttpPost]
        public ActionResult CrearHorarioSimple(CrearHorarioViewModel model)
        {
            using (var context = new CripcoEntities())
            {
                var idUsuario = ObtenerIdUsuario();
                var HorarioNuevo = new DateTime(model.FechaInicio.Year, model.FechaInicio.Month, model.FechaInicio.Day, model.FechaInicio.Hour,0,0);
                if (HorarioExistente(HorarioNuevo, idUsuario)) return Json(EnviarResultado(true, "El horario ya existe", "Si estaba inactivo se cambio a activo",""), JsonRequestBehavior.AllowGet);
                else 
                {
                    context.Horario.Add(new Horario {
                        PersonaID = idUsuario,
                        Hora = HorarioNuevo,
                        Reservado = false,
                        CreadoPor = User.Identity.Name,
                        FechaCreado = DateTime.Now,
                        ModificadoPor = User.Identity.Name,
                        Activo = true,
                        IdSala = model.IdSala
                    });
                    return Json(EnviarResultado(context.SaveChanges() > 0, "Crear Horario"), JsonRequestBehavior.AllowGet);
                }
            }
        }

        [HttpPost]
        public ActionResult CrearRangoHorario(CrearHorarioViewModel model)
        {
            using (var context = new CripcoEntities())
            {
                var idUsuario = ObtenerIdUsuario();
                var HoraInicio = new DateTime(model.HoraInicio.Year, model.HoraInicio.Month, model.HoraInicio.Day, model.HoraInicio.Hour, 0, 0);
                var HoraFinal = new DateTime(model.HoraFinal.Year, model.HoraFinal.Month, model.HoraFinal.Day, model.HoraFinal.Hour, 0, 0);
                var FechaInicio = new DateTime(model.FechaInicio.Year, model.FechaInicio.Month, model.FechaInicio.Day, 0, 0, 0);
                var FechaFinal = new DateTime(model.FechaFinal.Year, model.FechaFinal.Month, model.FechaFinal.Day, 0, 0, 0);

                var CantidadTiempo = FechaFinal - FechaInicio;
                var CantidadHoras = HoraFinal - HoraInicio;
                var Fecha = FechaInicio;
               
                for (var i = 0; i <= CantidadTiempo.Days; i++)
                {
                    var Hora = HoraInicio;
                    for (var j = 0; j <= CantidadHoras.Hours; j++)
                    {
                        var HorarioNuevo = new DateTime(Fecha.Year, Fecha.Month, Fecha.Day, Hora.Hour, 0, 0);
                        if (!HorarioExistente(HorarioNuevo, idUsuario))// { }// return Json(EnviarResultado(true, "El horario ya existe", "Si estaba inactivo se cambio a activo", ""), JsonRequestBehavior.AllowGet);
                        {
                            context.Horario.Add(new Horario
                            {
                                PersonaID = idUsuario,
                                Hora = HorarioNuevo,
                                Reservado = false,
                                CreadoPor = User.Identity.Name,
                                FechaCreado = DateTime.Now,
                                ModificadoPor = User.Identity.Name,
                                Activo = true,
                                IdSala = model.IdSala
                            });                           
                        }
                       Hora= Hora.AddHours(1);
                    }
                   Fecha= Fecha.AddDays(1);
                }
               return Json(EnviarResultado(context.SaveChanges() > 0, "Crear Horarios","Se crearon nuevos horarios","Horario Existente"), JsonRequestBehavior.AllowGet);               
            }
        }

        public bool HorarioExistente(DateTime HorarioNuevo, int idUsuario)
        {
            using (var context = new CripcoEntities())
            {
                foreach (var horario in context.Horario.Where(x=>x.PersonaID == idUsuario).ToList()) {
                    if (DateTime.Compare(HorarioNuevo, horario.Hora) == 0)
                    {
                        if (!horario.Activo)
                        {
                            horario.Activo = true;
                            context.SaveChanges();
                        }
                        return true;
                    }
                }
                return false;
            }
        }

        [HttpPost]
        public ActionResult CambiarEstado(int Id)
        {
            using (var context = new CripcoEntities())
            {
                var horario = context.Horario.Find(Id);
                horario.Activo = !horario.Activo;
                return Json(EnviarResultado(context.SaveChanges() > 0, "Cambiar Estado"), JsonRequestBehavior.AllowGet);
            }

        }

    }
}