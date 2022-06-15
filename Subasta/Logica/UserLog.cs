using Subasta.Models;
using Subasta.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Subasta.Logica
{
    public class UserLog
    {
        public User ConsultarUsuarios(string UserName, string Password)
        {
            RepositorioUser user = new RepositorioUser();
            return user.ConsultarUsuarios(UserName, Password);
        }

        public Respuesta AgregarUsuario(User user)
        {
            RepositorioUser userDAL = new RepositorioUser();
            Respuesta respuesta = new Respuesta();

            User usuario = new User();
            usuario = userDAL.CargarUsuarioPorNombreUsuario(user.UserName);
            if (usuario != null)
            {
                respuesta.Status = false;
                respuesta.Mensaje = "El Nombre de usuario ya existe, introduce un nombre diferente";
                return respuesta;
            }
            else
            {
                respuesta = userDAL.AgregarUsuario(user);
                return respuesta;
            }
        }


        public IEnumerable<User> CargarUsuariosPorSubasta(int AuctionId)
        {
            RepositorioUser user = new RepositorioUser();
            return user.CargarUsuariosPorSubasta(AuctionId);
        }
    }
}