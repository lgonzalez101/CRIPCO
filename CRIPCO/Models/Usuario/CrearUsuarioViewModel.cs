using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRIPCO.Models.Usuario
{
    public class CrearUsuarioViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Apellido")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "N° Identidad N°")]
        public int Identidad { get; set; }

        [Required]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime BirthDate { get; set; }

        [Required]
        [Display(Name = "N° de Telefono o Celular")]
        public int Telefono { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre de Usuario")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Correo Electronico")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Nueva Contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        [Compare("Password", ErrorMessage = "La nueva contraseña y la confirmacion no son las mismas")]
        public string ConfirmPassword { get; set; }

        public string IdAspNetUser { get; set; }
    }
}