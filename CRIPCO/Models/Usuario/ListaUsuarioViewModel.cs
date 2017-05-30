using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRIPCO.Models.Usuario
{
    public class ListaUsuarioViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Tipo de Usuario")]
        public string TipoUsuario { get; set; }

        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Display(Name="Fecha de Nacimiento")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; }
    }
}