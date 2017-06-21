using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRIPCO.Models.Expedientes
{
    public class ListaExpedienteViewModel
    {
        public int Id { get; set; }
        public string Paciente {get;set;}
        public string Comentario { get; set; }
        public byte[] Documento { get; set; }
        public bool Activo { get; set; }
}
}