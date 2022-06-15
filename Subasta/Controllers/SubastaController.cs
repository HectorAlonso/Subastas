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
    [ValidarSesion]
    public class SubastaController : Controller, IUsuario
    {
        public ActionResult Index(Auction auction)
        {
            IEnumerable<Auction> Subastas;
            AuctionLog auctionLog = new AuctionLog();

            if (auction.Description == null)
            {
                Subastas = auctionLog.CargarSubastas();
            }
            else
            {
                Subastas = auctionLog.CargarSubastasPorDescripcion(auction.Description);
            }
            
            return View(Subastas);
        }

        [HttpGet]
        public ActionResult Crear()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Crear(Auction subasta)
        {
            AuctionLog auctionLog = new AuctionLog();
            subasta.HighestBid = 0;
            subasta.Winner = 0;
            subasta.UserId = ObtenerUsuarioId();
            if (!ModelState.IsValid)
            {
                return View(subasta);
            }
            else
            {
                Respuesta respuesta = auctionLog.agregarSubasta(subasta);
                if (respuesta.Status)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(nameof(subasta.MensajeError), respuesta.Mensaje);
                    return View();
                }
            }
        }

        [HttpGet]
        public ActionResult Actualizar(int AuctionId)
        {
            AuctionLog auctionLog = new AuctionLog();
            var Subasta = auctionLog.CargarSubasta(AuctionId);

            if( Subasta is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            return View(Subasta);
        }

        [HttpPost]
        public ActionResult Actualizar(Auction subasta)
        {
            AuctionLog auctionLog = new AuctionLog();

            subasta.UserId = ObtenerUsuarioId();
            Respuesta respuesta = auctionLog.ModificarSubasta(subasta);
            if (respuesta.Status)
            {
                return View();
            }
            else
            {
                ModelState.AddModelError(nameof(subasta.MensajeError), respuesta.Mensaje);
                return View();
            }
        }

        public int ObtenerUsuarioId()
        {
            User Usuario = (User)Session["Usuario"];
            return Usuario.UserId;
        }
    }
}