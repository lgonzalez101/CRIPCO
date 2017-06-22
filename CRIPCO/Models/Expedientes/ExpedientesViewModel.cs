using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRIPCO.Models.Expedientes
{
    public class ExpedientesViewModel
    {
        public int Id { get; set; }
        public int CitaID { get; set; }
        [Required]
        public string Comentario { get; set; }
      
        public HttpPostedFileBase Documento { get; set; }
        public bool Activo { get; set; }
        public string ExtensionArchivo { get; set; }
    }
}