﻿using System;
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
        [Display(Name = "Tipo de Usuario")]
        public string Nombre { get; set; }

        public string Id { get; set; }
        public bool Estado { get; set; }
        public bool EsEditar { get; set; }

    }
}