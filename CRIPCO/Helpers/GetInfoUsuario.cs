﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CRIPCO.Helpers
{
    public static class GetInfoUsuario
    {
        static string Nombre { get; set; }
        static string TipoUsuario { get; set; }

        public static void GetInfo(string UserName)
        {
            using (var context = new CRIPCO.BD.CripcoEntities())
            {
                var info = context.Persona.FirstOrDefault(x => x.AspNetUsers.UserName == UserName);
                if (info != null)
                {
                    Nombre = info.Nombre + " " + info.Apellido;
                    TipoUsuario = info.AspNetUsers.AspNetRoles.FirstOrDefault().Name ?? "";
                }
            }
        }

        public static string ObtenerNombre()
        {
            return Nombre;
        }
        public static string ObtenerTipoUsuario()
        {
            return TipoUsuario;
        }
    }
}
