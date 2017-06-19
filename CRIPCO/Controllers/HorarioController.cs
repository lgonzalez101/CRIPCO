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
    [Authorize(Roles = "Doctor, Administrador")]
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
                var jsonResult = Json(context.Horario.Any(x => x.PersonaID == idUsuario) ? context.Horario.Where(x=>x.PersonaID== idUsuario)?.Select(x => new ListaHorarioViewModel { Id = x.HorarioID, Fecha = x.Hora, Estado = x.Activo }).ToList(): new List<ListaHorarioViewModel>(), JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }

        [HttpGet]
        public ActionResult MostrarCrearHorario()
        {
            return PartialView(new CrearHorarioViewModel { FechaInicio = DateTime.Now, FechaFinal = DateTime.Now });

        }

        [HttpPost]
        public ActionResult CrearRangoHorario(CrearHorarioViewModel model)
        {
            using (var context = new CripcoEntities())
            {
                return View();

            }
        }

        [HttpPost]
        public ActionResult CrearHorarioSimple(CrearHorarioViewModel model)
        {
            using (var context = new CripcoEntities())
            {
                var idUsuario = ObtenerIdUsuario();
                var HorarioNuevo = new DateTime(model.FechaInicio.Year, model.FechaInicio.Month, model.FechaInicio.Day, model.FechaInicio.Hour,0,0);
                var ListaHorarios = context.Horario.Where(x => x.Activo && x.PersonaID == idUsuario).ToList();
                var existe = false;
                foreach (var horario in ListaHorarios)
                {
                    if (DateTime.Compare(HorarioNuevo, horario.Hora) == 0)
                    {
                        existe = true;
                    }
                }
                if (!existe)
                {
                    context.Horario.Add(new Horario {
                        PersonaID = idUsuario,
                        Hora = HorarioNuevo,
                        Estado = "",
                        CreadoPor = User.Identity.Name,
                        FechaCreado = DateTime.Now,
                        ModificadoPor = User.Identity.Name,
                        Activo = true,

                    });
                }
                else {
                    return Json(EnviarResultado(false, "Crear Horario", "", "El Horario: " + HorarioNuevo.ToShortDateString() + " ya existe"), JsonRequestBehavior.AllowGet);
                }
                var result = context.SaveChanges()>0;
                return Json(EnviarResultado(result, "Crear Horario"), JsonRequestBehavior.AllowGet);


            }

        }





    }
}