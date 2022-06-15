using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Subasta.Models
{
    public class User
    {
        public int UserId { get; set; }
        
        [Display(Name ="Nombre de Usuario")]
        [Required(ErrorMessage ="El Nombre de Usuario es requerido")]
        public string UserName { get; set; }

        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "La Contraseña es requerida")]
        public string Password { get; set; }

        [Display(Name = "Nombre Completo")]
        [Required(ErrorMessage = "El Nombre es requerido")]
        public string Name { get; set; }

        [Display(Name = "Correo Electronico")]
        [Required(ErrorMessage ="El Correo es requerido")]
        [EmailAddress(ErrorMessage = "El campo {0} debe ser un correo electronico valido")]
        public string Email { get; set; }
    }
}