using Subasta.Models;
using Subasta.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Subasta.Logica
{
    public class AuctionRecordLog
    {
        public IEnumerable<AuctionRecord> CargarRegistroSubastas()
        {
            RepositorioAuctionRecord auctionRecordDAL = new RepositorioAuctionRecord();
            return auctionRecordDAL.CargarRegistroSubastas();
        }

        public IEnumerable<AuctionRecord> CargarRegistroSubastaPorUsuario(int UserId, int AuctionId)
        {
            RepositorioAuctionRecord auctionRecordDAL = new RepositorioAuctionRecord();
            return auctionRecordDAL.CargarRegistroSubastaPorUsuario(UserId, AuctionId);
        }

        public IEnumerable<AuctionRecord> CargarRegistroSubastasPorProducto(int AuctionId)
        {
            RepositorioAuctionRecord auctionRecordDAL = new RepositorioAuctionRecord();
            return auctionRecordDAL.CargarRegistroSubastasPorProducto(AuctionId);
        }
    }
}