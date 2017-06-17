﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CRIPCO.Models.Usuario
{
    public class ListaUsuarioViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Perfil")]
        public string Perfil { get; set; }

        [Display(Name = "Usuario")]
        public string UserName { get; set; }

        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Display(Name="Fecha de Nacimiento")]
        public DateTime? FechaNac { get; set; }

        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [Display(Name = "Estado")]
        public bool Estado { get; set; }
    }
}