using System;
using System.Collections.Generic;

namespace ConveniosApi.ConveniosMedicosEntities
{
    public partial class ConvenioMercado
    {
        public long Id { get; set; }
        public long ConvenioVersionId { get; set; }
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
        public decimal VentaActual { get; set; }
        public float Cumplimiento { get; set; }
        public string? Observaciones { get; set; }
        public string? ObservacionesVersion { get; set; }

        public virtual ConvenioVersione ConvenioVersion { get; set; } = null!;
    }
}
