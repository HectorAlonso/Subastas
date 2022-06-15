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
    public class RepositorioAuction
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionSubasta"].ToString();
        public Respuesta agregarSubasta(Auction auction)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                int id = connection.QuerySingle<int>(@"
                INSERT INTO Auction(ProductName, Description, StartDate, EndDate, UserId) 
                VALUES(@ProductName, @Description, @StartDate, @EndDate, @UserId);
                SELECT SCOPE_IDENTITY();", auction);
                Respuesta respuesta = new Respuesta();
                if (id > 0)
                {
                    respuesta.Status = true;
                    respuesta.Mensaje = "Subasta Agregada correctamente";
                }
                else
                {
                    respuesta.Status = false;
                    respuesta.Mensaje = "Ha ocurrido un error.";
                }
                return respuesta;
            }
        }

        public IEnumerable<Auction> CargarSubastas()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Auction>(@"
                SELECT AuctionId, ProductName, Description, StartDate, EndDate FROM Auction;");
            }
        }

        public IEnumerable<Auction> CargarSubastasPorDescripcion(string Descripcion)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Auction>(@"
                SELECT AuctionId, ProductName, Description, StartDate, EndDate 
                FROM Auction WHERE Description LIKE '%' + @Descripcion + '%';", new { Descripcion });
            }
        }

        public IEnumerable<Auction> CargarSubastasPorUsuario(int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<Auction>(@"
                SELECT AuctionId, ProductName, Description, StartDate, EndDate 
                FROM Auction WHERE UserId = @UserId;", new { UserId });
            }
        }

        public Auction CargarSubasta(int AuctionId)
        {
            using (var connection = new SqlConnection(connectionString))
            {//tRAER EL USUARIO ID
                return connection.QueryFirstOrDefault<Auction>(@"
                SELECT AuctionId, ProductName, Description, StartDate, EndDate, HighestBid, U.Name AS UserName, U.UserId as UserId
                FROM Auction
                INNER JOIN [dbo].[User] U
                ON Auction.UserId = U.UserId
                WHERE AuctionId = @AuctionId;", new { AuctionId });
            }
        }

        public Respuesta ModificarSubasta(Auction Auction)
        {
            Respuesta respuesta = new Respuesta();
            using (var connection = new SqlConnection(connectionString))
            {
                int id = connection.Execute(@"
                UPDATE Auction SET HighestBid = @HighestBid, UserId = @UserId
                WHERE AuctionId = @AuctionId;", new { Auction.HighestBid, Auction.UserId, Auction.AuctionId });
                if(id >= 1)
                {
                    respuesta.Status = true;
                    respuesta.Mensaje = "La oferta se ha realizado correctamente.";
                }
                else
                {
                    respuesta.Status = false;
                    respuesta.Mensaje = "La oferta no se ha podido actualizar";
                }
                return respuesta;
            }
        }

        public int CantidadSubastasActivasPorUsuario(int UserId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                var FechaActual = DateTime.Now;
                var Cantidad = connection.QueryFirstOrDefault<int>(@"
                SELECT COUNT(AuctionId) FROM Auction 
                WHERE UserId = @UserId AND @FechaActual < EndDate;", new { UserId, FechaActual });
                return Cantidad;
            }
        }
    }
}