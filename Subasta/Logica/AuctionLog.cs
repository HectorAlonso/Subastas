using Subasta.Models;
using Subasta.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;

namespace Subasta.Logica
{
    public class AuctionLog
    {
        public IEnumerable<Auction> CargarSubastas()
        {
            RepositorioAuction auction = new RepositorioAuction();
            return auction.CargarSubastas();
        }

        public Auction CargarSubasta(int ClaveSubasta)
        {
            RepositorioAuction auction = new RepositorioAuction();
            return auction.CargarSubasta(ClaveSubasta);
        }

        public IEnumerable<Auction> CargarSubastasPorUsuario(int ClaveUsuario)
        {
            RepositorioAuction auction = new RepositorioAuction();
            return auction.CargarSubastasPorUsuario(ClaveUsuario);
        }

        public IEnumerable<Auction> CargarSubastasPorDescripcion(string Descripcion)
        {
            RepositorioAuction auction = new RepositorioAuction();
            return auction.CargarSubastasPorDescripcion(Descripcion);
        }

        public Respuesta agregarSubasta(Auction auction)
        {
            RepositorioAuction auctionDal = new RepositorioAuction();
            IEnumerable<Auction> ListaSubastas;
            Respuesta respuesta = new Respuesta();

            ListaSubastas = auctionDal.CargarSubastasPorUsuario(auction.UserId);
            int SubastasActivas = auctionDal.CantidadSubastasActivasPorUsuario(auction.UserId);

            DateTime FechaAct = DateTime.Now.Date;
            DateTime FechaInicio = Convert.ToDateTime(auction.StartDate);
            DateTime FechaFin = Convert.ToDateTime(auction.EndDate);
            DateTime FechaNow = new DateTime(FechaAct.Year, FechaAct.Month, FechaAct.Day);
            DateTime FechaStart = new DateTime(FechaInicio.Year, FechaInicio.Month, FechaInicio.Day);
            DateTime FechaEnd = new DateTime(FechaFin.Year, FechaFin.Month, FechaFin.Day);
            //Fecha de inicio debe ser mayor o igual a la fecha actual
            int Resultado = DateTime.Compare(FechaStart, FechaNow);
            if (Resultado >= 0)
            {
                //la fecha fin debe ser mayor que la fecha de inicio
                int resul = DateTime.Compare(FechaEnd, FechaStart);
                if (resul > 0)
                {
                    if (SubastasActivas < 3)
                    {
                        return auctionDal.agregarSubasta(auction);
                    }
                    else
                    {
                        respuesta.Status = false;
                        respuesta.Mensaje = "No puede tener mas de 3 subastas activas";
                        return respuesta; 
                    }
                }
                else
                {
                    respuesta.Status = false;
                    respuesta.Mensaje = "La fecha final no es mayor que la fecha de inicio";
                    return respuesta;
                }
            }
            else
            {
                respuesta.Status = false;
                respuesta.Mensaje = "La fecha inicio no es mayor o igual que la fecha actual";
                return respuesta;
            }
        }

        public Respuesta ModificarSubasta(Auction pAction)
        {
            RepositorioAuction auctionDal = new RepositorioAuction();
            Auction auction = new Auction();
            Respuesta respuesta = new Respuesta();

            RepositorioAuctionRecord aucrecDAL = new RepositorioAuctionRecord();
            AuctionRecord AucRec = new AuctionRecord();

            auction = auctionDal.CargarSubasta(pAction.AuctionId);

            if (auction != null)
            {
                if (pAction.HighestBid > 0 && pAction.HighestBid < 1000000)
                {
                    if (pAction.HighestBid > auction.HighestBid)
                    {
                        DateTime Fecha1 = DateTime.Now.Date;
                        DateTime Fecha2 = Convert.ToDateTime(auction.EndDate);
                        DateTime FechaActual = new DateTime(Fecha1.Year, Fecha1.Month, Fecha1.Day);
                        DateTime FechaFinal = new DateTime(Fecha2.Year, Fecha2.Month, Fecha2.Day);
                        int Resultado = DateTime.Compare(FechaActual, FechaFinal);
                        // menor a 0 si la fecha1 es menor a la fecha 2
                        // 0 si las fechas son iguales
                        //mayor a 0 si la fecha 1 es mayor a la fecha 2
                        if (Resultado < 0)
                        {
                            if (pAction.UserId != auction.UserId)
                            {
                                using (TransactionScope ts = new TransactionScope())
                                {
                                    respuesta = auctionDal.ModificarSubasta(pAction);
                                    AucRec.AuctionId = pAction.AuctionId;
                                    AucRec.UserId = pAction.UserId;
                                    AucRec.Amount = decimal.Parse(pAction.HighestBid.ToString());
                                    AucRec.BidDate = DateTime.Now.Date;

                                    aucrecDAL.AgregarAuctionRecord(AucRec);

                                    ts.Complete();
                                }
                                return respuesta;

                            }
                            else
                            {
                                respuesta.Status = false;
                                respuesta.Mensaje = "No se le permite ofertar en esta subasta.";
                                return respuesta;
                            }
                        }
                        else
                        {
                            respuesta.Status = false;
                            respuesta.Mensaje = "La subasta ha finalizado.";
                            return respuesta;
                            
                        }
                    }
                    else
                    {
                        respuesta.Status = false;
                        respuesta.Mensaje = "La oferta debe ser mayor a la oferta actual.";
                        return respuesta;
                    }
                }
                else
                {
                    respuesta.Status = false;
                    respuesta.Mensaje = "Cantidad no permitida, debe ser mayor a 0 y menor a 1,000,000.";
                    return respuesta;
                }
            }
            else
            {
                respuesta.Status = false;
                respuesta.Mensaje = "No existe una Subasta con esa clave.";
                return respuesta;
            }
        }
    }
}