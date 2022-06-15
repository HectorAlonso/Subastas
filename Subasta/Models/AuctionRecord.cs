using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Subasta.Models
{
    public class AuctionRecord
    {
        public int RecordId { get; set; }
        public int AuctionId { get; set; }
        public int UserId { get; set; }
        public decimal Amount { get; set; }

        public DateTime BidDate { get; set; }

        //Propiedad extra para mostrar el Nombre del usuario
        public string UserName { get; set; }
    }
}