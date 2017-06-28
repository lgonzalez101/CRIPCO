using CRIPCO.BD;
using CRIPCO.Models.Calendario;
using CRIPCO.Models.Horario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRIPCO.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }




        public ActionResult VerHorariosDelDia()
        {
            using (var context = new CripcoEntities())
            {
                var IdUsuario = ObtenerIdUsuario();
                var listaHorarios = context.Horario.Any(x => x.PersonaID == IdUsuario && x.Activo) ? context.Horario.Where(x => x.PersonaID == IdUsuario && x.Activo)?.Select(x => new ListaHorarioViewModel { Id = x.HorarioID, Fecha = x.Hora, Estado = x.Activo, Reservado = x.Reservado }).ToList(): new List<ListaHorarioViewModel>();
                var listfinal = new List<ListaHorarioViewModel>();
                listaHorarios.ForEach(x =>
                {
                    if (x.Fecha.Date == DateTime.Now.Date)
                    {
                        listfinal.Add(x);
                    }
                });
              
                return PartialView(listfinal);

            }

        }
        

      public ActionResult VerCitasDelPaciente()
        {
            using (var context = new CripcoEntities())
            {
                var IdUsuario = ObtenerIdUsuario();
                var listaHorarios = context.Cita.Any(x => x.PersonaPacienteID == IdUsuario && x.Activo) ? context.Cita.Where(x => x.PersonaPacienteID == IdUsuario && x.Activo)?.Select(x => new ListaHorarioViewModel { Id = x.HorarioID, Fecha = x.Horario.Hora, Estado = x.Activo, Reservado = x.Horario.Reservado }).ToList() : new List<ListaHorarioViewModel>();
                var listfinal = new List<ListaHorarioViewModel>();
                listaHorarios.ForEach(x =>
                {
                    if (x.Fecha.Date > DateTime.Now.Date)
                    {
                        listfinal.Add(x);
                    }
                });

                return PartialView(listfinal);

            }

        }


    }
}