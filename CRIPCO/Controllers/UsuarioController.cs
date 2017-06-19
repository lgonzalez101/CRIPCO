using CRIPCO.BD;
using CRIPCO.Models;
using CRIPCO.Models.Base;
using CRIPCO.Models.Usuario;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CRIPCO.Controllers
{
    public class UsuarioController : BaseController
    {
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        // GET: Usuario
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListarUsuarios()
        {
            try
            {
                using (var context = new CripcoEntities())
                {
                    var listaUsuarios = context.Persona.Select(x => new ListaUsuarioViewModel {
                        Id = x.PersonaID,
                        Nombre = x.Nombre,
                        Apellido = x.Apellido,
                        FechaNac = x.FechaNac,
                        Email = x.AspNetUsers.Email,
                        Estado = x.Activo,
                        Perfil = x.AspNetUsers.AspNetRoles.Any() ? x.AspNetUsers.AspNetRoles.FirstOrDefault().Name : "",
                        UserName = x.AspNetUsers.UserName
                    }).ToList();

                    var jsonResult = Json(listaUsuarios, JsonRequestBehavior.AllowGet);
                    jsonResult.MaxJsonLength = Int32.MaxValue;
                    return jsonResult;

                }
            }
            catch(Exception e) {
                return View();
            }
        }


        [HttpGet]
        public ActionResult CrearUsuario()
        {
            using (var context = new CripcoEntities())
            {
                ViewBag.ListaTipoUsuario = context.AspNetRoles.Select(x => new SelectListItem { Value = x.Id, Text = x.Name }).ToList();
                return View(new CrearUsuarioViewModel { FechaNac = DateTime.Now });
            }
           
        }

        [HttpPost]
        public async Task<ActionResult> CrearUsuario(CrearUsuarioViewModel model)
        {
            try
            {
                using (var context = new CripcoEntities())
                {
                    var user = new ApplicationUser { UserName = model.UserName.Trim(), Email = model.Email.Trim() };
                    var result = await UserManager.CreateAsync(user, model.Password.Trim());
                    if (result.Succeeded)
                    {
                        await UserManager.AddToRoleAsync(user.Id, context.AspNetRoles.Find(model.RoleUsuario).Name);
                        var test = context.Persona.Add(new Persona
                        {
                            Nombre = model.Nombre.Trim(),
                            Apellido = model.Apellido.Trim(),
                            FechaNac = model.FechaNac,
                            IdAspnetUser = user.Id,
                            Telefono = model.Telefono,
                            Identidad = model.Identidad,
                            Activo=true,
                            CreadoPor = User.Identity.Name,
                            ModificadoPor = User.Identity.Name,
                            FechaCreado =  DateTime.Now,
                           
                        });

                        var resultado = context.SaveChanges() > 0;
                        return Json(EnviarResultado(resultado, "Crear Usuario"), JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(EnviarResultado(result.Succeeded, result.Errors.FirstOrDefault()), JsonRequestBehavior.AllowGet);

                    }
                }
            }
            catch (Exception e)
            {
                return Json( EnviarResultado(false, e.Message), JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult EditarInfoUsuario(int Id)
        {
            using (var context = new CripcoEntities())
            {
                var usuario = context.Persona.Find(Id);
                return PartialView(new CrearUsuarioViewModel
                {
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    FechaNac = usuario.FechaNac,
                    Telefono = usuario.Telefono,
                    Identidad = usuario.Identidad
                });
            }
        }

        [HttpPost]
        public ActionResult EditarInfoUsuario(CrearUsuarioViewModel model)
        {
            using (var context = new CripcoEntities())
            {
                var usuario = context.Persona.Find(model.Id);
                usuario.Nombre = model.Nombre;
                usuario.Apellido = model.Apellido;
                usuario.FechaNac = model.FechaNac;
                usuario.Telefono = model.Telefono;
                usuario.Identidad = model.Identidad;
                usuario.ModificadoPor = User.Identity.Name;
                context.Entry(usuario).State = EntityState.Modified;
                var result = context.SaveChanges() > 0;
                return Json(new MensajeRespuestaViewModel
                {
                    Titulo = "Editar Usuario",
                    Mensaje = result ? "Se edito Satisfactoriamente" : "Error al editar el usuario",
                    Estado = result
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public async Task<ActionResult> EditarCuentaUsuario(int Id)
        {
            using (var context = new CripcoEntities())
            {
                ViewBag.ListaTipoUsuario = context.AspNetRoles.Select(x => new SelectListItem { Value = x.Id, Text = x.Name }).ToList();
                var usuario = context.Persona.Find(Id);
                var roles = await UserManager.GetRolesAsync(usuario.IdAspnetUser);
                return PartialView(new CrearUsuarioViewModel
                {
                    Id = usuario.PersonaID,
                    UserName = usuario.AspNetUsers.UserName,
                    Email = usuario.AspNetUsers.Email,
                    IdAspNetUser = usuario.IdAspnetUser,
                    RoleUsuario = usuario.AspNetUsers.AspNetRoles.FirstOrDefault()?.Id??"",                 
                });

            }
        }


        [HttpPost]
        public async Task<ActionResult> EditarCuentaUsuario(CrearUsuarioViewModel model)
        {
            try
            {
                using (var context = new CripcoEntities())
                {
                    var usuario = context.Persona.Find(model.Id);
                    usuario.AspNetUsers.UserName = model.UserName;
                    usuario.AspNetUsers.Email = model.Email;
                    context.Entry(usuario).State = System.Data.Entity.EntityState.Modified;
                    var roles = await UserManager.GetRolesAsync(usuario.AspNetUsers.Id);
                    await UserManager.RemoveFromRolesAsync(usuario.AspNetUsers.Id, roles.ToArray());
                    var result2 = await UserManager.AddToRoleAsync(usuario.AspNetUsers.Id, context.AspNetRoles.Find(model.RoleUsuario).Name);
                   
                    var result = context.SaveChanges() > 0;
                    return Json(new MensajeRespuestaViewModel
                    {
                        Titulo = "Editar Usuario",
                        Mensaje = result && result2.Succeeded ? "Se edito Satisfactoriamente" : "Error al editar el usuario",
                        Estado = result
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new MensajeRespuestaViewModel
                {
                    Titulo = "Editar Usuario",
                    Mensaje = "Error al editar el usuario",
                    Estado = false
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult DeshabilitarUsuario(int Id)
        {
            using (var context = new CripcoEntities())
            {

                var usuario = context.Persona.Find(Id);
                usuario.Activo = false;
                           
                var result = context.SaveChanges() > 0;
                return Json(new MensajeRespuestaViewModel
                {
                    Titulo = "Deshabilitar Usuario",
                    Mensaje = result ? "Se deshabilito Satisfactoriamente" : "Error al deshabilitar el usuario",
                    Estado = result
                }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpGet]
        public ActionResult ResetContrasena(int Id)
        {
            using (var context = new CripcoEntities())
            {
                return PartialView(new CambiarContrasenaViewModel { IdUser = Id });
            }
        }


        [HttpPost]
        public async Task<ActionResult> ResetContrasena(CambiarContrasenaViewModel model)
        {
            using (var context = new CripcoEntities())
            {
                var User = context.Persona.Find(model.IdUser);
                string code = await UserManager.GeneratePasswordResetTokenAsync(User.IdAspnetUser);
                var result = await UserManager.ResetPasswordAsync(User.IdAspnetUser, code, model.NewPassword);

                return Json(new MensajeRespuestaViewModel
                {
                    Titulo = "Cambiar Contrasena",
                    Mensaje = result.Succeeded ? "Se cambio Satisfactoriamente" : "Error al cambiar la contrasena",
                    Estado = result.Succeeded
                }, JsonRequestBehavior.AllowGet);
            }
        }







    }
}