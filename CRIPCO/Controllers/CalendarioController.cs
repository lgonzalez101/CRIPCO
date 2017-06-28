using CRIPCO.BD;
using CRIPCO.Models.Calendario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRIPCO.Controllers
{
    public class CalendarioController : Controller
    {
        // GET: Calendario
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCalendarioHorarios()
        {
            using (var context = new CripcoEntities())
            {
                var listaContratos = context.Horario.Where(x => x.Activo).ToList();
                var listaEventosContratos =
                    listaContratos.Select(
                        x =>
                            new CalendarioViewModel
                            {
                                className = new string[] { "event", x.Reservado ? "bg-red" : "bg-blue" },
                                icon = x.Reservado ? "fa-lock" : "fa-check",
                                description = $"Informacion: {x.Hora.ToShortDateString()}",
                                id = x.HorarioID,
                                start = x.Hora,
                                title = $"Titulo:"
                            }).ToList();
                var jsonResult = Json(listaEventosContratos, JsonRequestBehavior.AllowGet);
                jsonResult.MaxJsonLength = Int32.MaxValue;
                return jsonResult;
            }
        }
    }
}