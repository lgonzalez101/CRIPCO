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
    
    public partial class Persona
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Persona()
        {
            this.Citas = new HashSet<Cita>();
            this.Horarios = new HashSet<Horario>();
            this.PersonaEspecialidads = new HashSet<PersonaEspecialidad>();
        }
    
        public int PersonaID { get; set; }
        public string IdAspnetUser { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Identidad { get; set; }
        public System.DateTime FechaNac { get; set; }
        public string Telefono { get; set; }
        public string CreadoPor { get; set; }
        public System.DateTime FechaCreado { get; set; }
        public string ModificadoPor { get; set; }
        public bool Activo { get; set; }
    
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Cita> Citas { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Horario> Horarios { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PersonaEspecialidad> PersonaEspecialidads { get; set; }
    }
}
