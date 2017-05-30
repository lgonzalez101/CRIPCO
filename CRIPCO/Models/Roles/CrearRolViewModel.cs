using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRIPCO.Models.Roles
{
    public class CrearRolViewModel
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre de Rol")]
        public string Nombre { get; set; }
    }
}