using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRIPCO.Models.Horario
{
    public class ListaHorarioViewModel
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public bool Reservado { get; set; }
        public bool Estado { get; set; }
        public int IdCita { get; set; }
    }
}