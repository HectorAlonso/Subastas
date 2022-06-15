using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Subasta.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "El Nombre de usuario es requerido")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "La contraseña es requerido")]
        public string Password { get; set; }
    }
}