using System;
using System.Collections.Generic;

namespace ConveniosApi.ConveniosMedicosEntities
{
    public partial class ConvenioPagoDetalle
    {
        public long Id { get; set; }
        public long ConvenioPagoId { get; set; }
        public string Mercado { get; set; } = null!;
        public short MdoAnt { get; set; }
        public short MdoAct { get; set; }
        public short MdoUltimo { get; set; }
        public short AcxAnt { get; set; }
        public short AcxAct { get; set; }
        public short AcxUltimo { get; set; }
        public float Objetivo { get; set; }
        public decimal VentaInicial { get; set; }
        public decimal VentaObjetivo { get; set; }
        public decimal VentaAlcanzada { get; set; }
        public decimal MontoNegociado { get; set; }
        public float Cumplimiento { get; set; }
        public decimal MontoCalculado { get; set; }
        public float? CumplimientoJustificado { get; set; }
        public string? Justificacion { get; set; }
        public decimal? MontoJustificado { get; set; }

        public virtual ConvenioPago ConvenioPago { get; set; } = null!;
    }
}
