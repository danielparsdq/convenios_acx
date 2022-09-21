using System;
using System.Collections.Generic;

namespace ConveniosApi.ConveniosMedicosEntities
{
    public partial class ConvenioPago
    {
        public ConvenioPago()
        {
            ConvenioPagoDetalles = new HashSet<ConvenioPagoDetalle>();
        }

        public long Id { get; set; }
        public long ConvenioVersionId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaUltimosDatos { get; set; }
        public long Solicitante { get; set; }
        public decimal MontoSolicitado { get; set; }
        public decimal MontoCalculado { get; set; }
        public decimal? MontoJustificado { get; set; }
        public string RazonSocial { get; set; } = null!;
        public string Ruc { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public float Iva { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public string? DocumentoFactura { get; set; }

        public virtual ConvenioVersione ConvenioVersion { get; set; } = null!;
        public virtual ICollection<ConvenioPagoDetalle> ConvenioPagoDetalles { get; set; }
    }
}
