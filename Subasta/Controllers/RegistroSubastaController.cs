using Subasta.Logica;
using Subasta.Models;
using Subasta.Permisos;
using Subasta.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Subasta.Controllers
{
    [ValidarSesion]
    public class RegistroSubastaController : Controller, IUsuario
    {
        // GET: RegistroSubasta
        public ActionResult Index(int AuctionId)
        {
            AuctionLog AuctionLog = new AuctionLog();
            Auction Subasta = AuctionLog.CargarSubasta(AuctionId);
            CargarUsuarios(AuctionId);
            int UserId = ObtenerUsuarioId();
            ViewBag.ListaRegistroSubastas = CargarRegistroSubastasPorUsuario(UserId, AuctionId);
            return View(Subasta);
        }

        public void CargarUsuarios(int Subasta)
        {
            List<SelectListItem> ListaUsuarios = new List<SelectListItem>();
            UserLog UserLog = new UserLog();
            var usuarios = UserLog.CargarUsuariosPorSubasta(Subasta);
            foreach (var usuario in usuarios)
            {
                ListaUsuarios.Add(new SelectListItem
                {
                    Text = usuario.UserName,
                    Value = usuario.UserId.ToString()
                });
            }
            ListaUsuarios.Insert(0, new SelectListItem { Text = "Seleccione", Value = "" });
            ViewBag.ListaUsuarios = ListaUsuarios;
        }

        public IEnumerable<AuctionRecord> CargarRegistroSubastasPorUsuario(int UserId, int AuctionId)
        {
            AuctionRecordLog auctionRecordLog = new AuctionRecordLog();
            IEnumerable<AuctionRecord> ListaRegistros;
            if(UserId == 0)
            {
                ListaRegistros = auctionRecordLog.CargarRegistroSubastasPorProducto(AuctionId);
            }
            else
            {
                ListaRegistros = auctionRecordLog.CargarRegistroSubastaPorUsuario(UserId, AuctionId);
            }
             return ListaRegistros;
        }

        [HttpPost]
        public JsonResult CargarRecords(AuctionRecord record)
        {
            var records = CargarRegistroSubastasPorUsuario(record.UserId, record.AuctionId);
            return Json(records, JsonRequestBehavior.AllowGet);
        }

        public int ObtenerUsuarioId()
        {
            User Usuario = (User)Session["Usuario"];
            return Usuario.UserId;
        }
    }
}