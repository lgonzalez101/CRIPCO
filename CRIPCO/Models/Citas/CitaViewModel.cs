using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRIPCO.Models.Citas
{
    public class CitaViewModel
    {

        public int CitaID { get; set; }
        public int HorarioID { get; set; }
        public int PersonaPacienteID { get; set; }
        public string CreadoPor { get; set; }
        public System.DateTime FechaCreado { get; set; }
        public string ModificadoPor { get; set; }
        public bool Activo { get; set; }
    }
}