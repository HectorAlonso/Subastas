using Dapper;
using Subasta.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Subasta.Servicios
{
    public class RepositorioUser
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionSubasta"].ToString();

        public User ConsultarUsuarios(string UserName, string Password)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<User>(@"
                SELECT UserId, UserName, Name, Email FROM [dbo].[User]
                WHERE UserName = @UserName AND Password = @Password;", new { UserName, Password });
            }
        }

        public Respuesta AgregarUsuario(User user)
        {
            Respuesta respuesta = new Respuesta();
            using (var connection = new SqlConnection(connectionString))
            {
                var id = connection.QuerySingle<int>(@"
                INSERT INTO [dbo].[User] VALUES(@UserName, @Password, @Name, @Email);
                SELECT SCOPE_IDENTITY();", user);
                if (id >= 1)
                {
                    respuesta.Status = true;
                    respuesta.Mensaje = "El usuario se ha registrado correctamente.";
                }
                else
                {
                    respuesta.Status = false;
                    respuesta.Mensaje = "El usuario no se ha podido registrar";
                }
                return respuesta;
            }
        }

        public User CargarUsuarioPorNombreUsuario(string UserName)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.QueryFirstOrDefault<User>(@"
                SELECT UserId, UserName, Name, Email FROM [dbo].[User]
                WHERE UserName = @UserName;", new { UserName });
            }
        }

        public IEnumerable<User> CargarUsuariosPorSubasta(int AuctionId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<User>(@"
                SELECT Distinct(U.UserName), U.UserId FROM AuctionRecord
                INNER JOIN [dbo].[User] AS U
                ON AuctionRecord.UserId = U.UserId
                WHERE AuctionRecord.AuctionId = @AuctionId", new { AuctionId });
            }
        }

    }
}