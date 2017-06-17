using CRIPCO.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CRIPCO.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public ActionResult Index()
        {
            return View();
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
    }
}