using Subasta.Logica;
using Subasta.Models;
using Subasta.Permisos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Subasta.Controllers
{
    
    public class UserController : Controller
    {
        // GET: User
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            UserLog userLog = new UserLog();
            User Usuario = new User();

            Usuario = userLog.ConsultarUsuarios(user.UserName, user.Password);

            if(Usuario != null)
            {
                Session["Usuario"] = Usuario;
                return RedirectToAction("Index", "Subasta");
            }
            else
            {
                ModelState.AddModelError(nameof(user.Password), "El Nombre de Usuario y/o contraseña son incorrectos");
                return View();
            }
        }

        [HttpGet]
        public ActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrarse(User Usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(Usuario);
            }

            UserLog userLog = new UserLog();

            Respuesta respuesta = userLog.AgregarUsuario(Usuario);

            if (respuesta.Status)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                ModelState.AddModelError(nameof(Usuario.UserName), "El Nombre de Usuario ya existe, intente con otro.");
                return View();
            }
        }


        [HttpPost]
        public ActionResult Logout()
        {
            Session["Usuario"] = null;
            return RedirectToAction("Login", "User");
        }
    }
}