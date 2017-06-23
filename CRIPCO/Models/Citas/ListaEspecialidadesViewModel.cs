using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRIPCO.Models.Citas
{
    public class ListaEspecialidadesViewModel
    {
        public int EspecialidadID { get; set; }
        public string Nombre { get; set; }
        public bool Activo { get; set; }
    }
}