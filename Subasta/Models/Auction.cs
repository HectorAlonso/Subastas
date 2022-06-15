using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Subasta.Models
{
    public class Auction
    {
        [Display(Name ="Clave Subasta")]
        public int AuctionId { get; set; }

        [Display(Name = "Nombre de Producto")]
        [Required(ErrorMessage = "El campo Nombre Producto es requerido.")]
        [StringLength(maximumLength: 50, ErrorMessage = "El limite permitido es de {1} caracteres.")]
        public string ProductName { get; set; }

        [Display(Name = "Descripcion del Producto")]
        [Required(ErrorMessage = "El campo Descripcion es requerido.")]
        [StringLength(maximumLength: 100, ErrorMessage = "El limite permitido es de {1} caracteres.")]
        public string Description { get; set; }

        [Display(Name = "Fecha de Inicio")]
        [Required(ErrorMessage = "El campo Fecha de Inicio es requerido.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Fecha Fin")]
        [Required(ErrorMessage = "El campo Fecha Fin es requerido.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        public decimal HighestBid { get; set; }
        public int Winner { get; set; }
        public int UserId { get; set; }

        //Propiedades extras para Joins
        public string UserName { get; set; }

        public string MensajeError { get; set; }
    }
}