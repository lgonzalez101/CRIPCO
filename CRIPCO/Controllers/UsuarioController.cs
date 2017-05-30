using CRIPCO.Models;
using CRIPCO.Models.Base;
using CRIPCO.Models.Roles;
using CRIPCO.Models.Usuario;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CRIPCO.Controllers
{
    public class UsuarioController : Controller
    {
        // GET: Usuario
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

        [HttpGet]
        public ActionResult CrearUsuario()
        {
           return View();
            
        }

        [HttpPost]
        public async Task<ActionResult> CrearUsuario(CrearUsuarioViewModel model)
        {
           
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult CrearRol()
        {
            return View();

        }

        [HttpPost]
        public async Task<ActionResult> CrearRol(CrearRolViewModel model)
        {
            return RedirectToAction("Index");
        }





    }
}