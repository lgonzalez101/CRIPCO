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
    
    public partial class Sala
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sala()
        {
            this.Horarios = new HashSet<Horario>();
        }
    
        public int SalaID { get; set; }
        public Nullable<int> AreaID { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public string CreadoPor { get; set; }
        public Nullable<System.DateTime> FechaCreado { get; set; }
        public string ModificadoPor { get; set; }
        public Nullable<bool> Activo { get; set; }
    
        public virtual Area Area { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Horario> Horarios { get; set; }
    }
}
