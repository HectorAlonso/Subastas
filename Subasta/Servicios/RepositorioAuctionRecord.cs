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
    public class RepositorioAuctionRecord
    {
        string connectionString = ConfigurationManager.ConnectionStrings["ConnectionSubasta"].ToString();

        public void AgregarAuctionRecord(AuctionRecord auctionRecord )
        {
            using (var connection = new SqlConnection(connectionString))
            {
                int id = connection.QuerySingle<int>(@"
                INSERT INTO AuctionRecord VALUES(@AuctionId, @UserId, @Amount, @BidDate);
                SELECT SCOPE_IDENTITY();", auctionRecord);
            }
        }

        public IEnumerable<AuctionRecord> CargarRegistroSubastas()
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<AuctionRecord>(@"
                SELECT RecordId, AuctionId, UserId, Amount, BidDate FROM AuctionRecord;");
            }
        }

        public IEnumerable<AuctionRecord> CargarRegistroSubastaPorUsuario(int UserId, int AuctionId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<AuctionRecord>(@"
                SELECT RecordId, AuctionId, AR.UserId, Amount, BidDate, U.Name as UserName
                FROM AuctionRecord AR
                INNER JOIN [dbo].[User] U
                ON AR.UserId = U.UserId
                WHERE AR.UserId = @UserId AND AuctionId = @AuctionId;",
                new { UserId, AuctionId });
            }
        }

        public IEnumerable<AuctionRecord> CargarRegistroSubastasPorProducto(int AuctionId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                return connection.Query<AuctionRecord>(@"
                SELECT RecordId, AuctionId, AR.UserId, Amount, BidDate, U.Name as UserName
                FROM AuctionRecord AR
                INNER JOIN [dbo].[User] U
                ON AR.UserId = U.UserId
                WHERE AuctionId = @AuctionId;", new { AuctionId });
            }
        }
    }
}