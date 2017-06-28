using CRIPCO.BD;
using CRIPCO.Models;
using CRIPCO.Models.Base;
using CRIPCO.Models.Usuario;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CRIPCO.Controllers
{
    [Authorize(Roles = "Administrador")]
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
            var name = User.Identity.Name;
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
                        UserName = x.AspNetUsers.UserName,
                        esDoctor = x.AspNetUsers.AspNetRoles.Any(z=>z.Name=="Doctor")
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

        public ActionResult ObtenerListaEspecialidades(int Id)
        {
            try
            {
                using (var context = new CripcoEntities())
                {
                    ViewBag.IdPersona = Id;
                    var listarEspecialidades = context.Especialidad.Select(x => new SeleccionEspecialidadesViewModel {

                        id = x.EspecialidadID,
                        nombre=x.Nombre,
                        especialidadSeleccionada= x.PersonaEspecialidad.Any(y => y.PersonaID == Id)
                    }).ToList();

                    return PartialView(listarEspecialidades);

                }
            }
            catch (Exception e)
            {
                return View();
            }
        }

        public ActionResult GuardarEspecialidadesEnUsuario(List<int> Lista, int IdPersona)
        {
            try
            {
                using (var context = new CripcoEntities())
                {
                    if (Lista != null)
                    {
                        var listaborrar= context.PersonaEspecialidad.Where(x => x.PersonaID == IdPersona).ToList();
                        if(listaborrar !=null) context.PersonaEspecialidad.RemoveRange(listaborrar);
                        Lista.ForEach(x => {  context.PersonaEspecialidad.Add(new PersonaEspecialidad { EspecialidadID = x, PersonaID = IdPersona }); } );   
                    }

                    var resultado = context.SaveChanges() > 0;
                    return Json(new MensajeRespuestaViewModel
                    {
                        Titulo = "Asignar Especialidad Al Usuario",
                        Mensaje = resultado ? "Se guardo Satisfactoriamente" : "No se asignaron especialidades al usuario",
                        Estado = resultado
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                return Json(new MensajeRespuestaViewModel
                {
                    Titulo = "No se asignaron especialidades al usuario",
                    Mensaje = e.InnerException.Message,
                    Estado = false
                }, JsonRequestBehavior.AllowGet);
            }

        }
        
        [HttpGet]
        public ActionResult CrearUsuario()
        {
            using (var context = new CripcoEntities())
            {
                ViewBag.ListaTipoUsuario = context.AspNetRoles.Where(x=>x.Activo??false).Select(x => new SelectListItem { Value = x.Id, Text = x.Name }).ToList();
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
                ViewBag.ListaTipoUsuario = context.AspNetRoles.Where(x=>x.Activo??false).Select(x => new SelectListItem { Value = x.Id, Text = x.Name }).ToList();
                var usuario = context.Persona.Find(Id);
                var roles = await UserManager.GetRolesAsync(usuario.IdAspnetUser);
                return PartialView(new CrearUsuarioViewModel
                {
                    Id = usuario.PersonaID,
                    UserName = usuario.AspNetUsers.UserName,
                    Email = usuario.AspNetUsers.Email,
                    IdAspNetUser = usuario.IdAspnetUser,
                    RoleUsuario = usuario.AspNetUsers.AspNetRoles.FirstOrDefault()?.Id??"",  
                    Estado = usuario.Activo               
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
                    usuario.Activo = model.Estado;
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

        
        [HttpGet]
        public ActionResult DetalleUsuario()
        {
            using (var context = new CripcoEntities())
            {
                int idUsuario = ObtenerIdUsuario();
                var Usuario = context.Persona.FirstOrDefault(x => x.PersonaID == idUsuario);

                var  detalleUsuario = new DetalleUsuarioViewModel { PersonaID = Usuario.PersonaID,
                                                                    Nombre = Usuario.Nombre,
                                                                    Apellido=Usuario.Apellido,
                                                                    Identidad=Usuario.Identidad,
                                                                    FechaNac= Usuario.FechaNac,
                                                                    Telefono=Usuario.Telefono,
                                                                  };
                return View(detalleUsuario);
               
            }

        }





    }
}