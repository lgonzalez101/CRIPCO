using CRIPCO.Models.Base;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace CRIPCO.Controllers
{
    public class BaseController : Controller
    {

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");
            base.OnActionExecuted(filterContext);
        }


        public MensajeRespuestaViewModel  EnviarResultado(bool resultado, string Titulo)
        {
            return new MensajeRespuestaViewModel
            {
                Titulo = Titulo,
                Mensaje = resultado ? "Accion exitosa" : "Error",
                Estado = resultado
            };
        }

        public MensajeRespuestaViewModel EnviarResultado(bool resultado, string Titulo, string MensajeExitoso, string MensajeError)
        {
            return new MensajeRespuestaViewModel
            {
                Titulo = Titulo,
                Mensaje = resultado ? MensajeExitoso : MensajeError,
                Estado = resultado
            };

        }

        public int ObtenerIdUsuario()
        {
            using (var context = new CRIPCO.BD.CripcoEntities())
            {
                return context.Persona.FirstOrDefault(x => x.AspNetUsers.UserName == User.Identity.Name)?.PersonaID??0;

            }
        }
    }
}