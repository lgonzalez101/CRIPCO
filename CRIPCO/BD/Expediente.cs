//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CRIPCO.BD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Expediente
    {
        public int ExpedienteID { get; set; }
        public int CitaID { get; set; }
        public string Comentario { get; set; }
        public byte[] Documento { get; set; }
        public string CreadoPor { get; set; }
        public System.DateTime FechaCreado { get; set; }
        public string ModificadoPor { get; set; }
        public bool Activo { get; set; }
        public string ExtensionDocumento { get; set; }
    
        public virtual Cita Cita { get; set; }
    }
}
