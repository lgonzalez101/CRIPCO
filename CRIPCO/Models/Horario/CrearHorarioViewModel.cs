using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRIPCO.Models.Horario
{
    public class CrearHorarioViewModel
    {
        public int IdUsuario { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFinal { get; set; }
        public DateTime HoraInicio { get; set; }
        public DateTime HoraFinal { get; set; }
        public int IdSala { get; set; }
    }
}