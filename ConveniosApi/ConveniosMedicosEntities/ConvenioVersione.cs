using System;
using System.Collections.Generic;

namespace ConveniosApi.ConveniosMedicosEntities
{
    public partial class ConvenioVersione
    {
        public ConvenioVersione()
        {
            ConvenioMercados = new HashSet<ConvenioMercado>();
            ConvenioPagos = new HashSet<ConvenioPago>();
            ConveniosInstitucionesVenta = new HashSet<ConveniosInstitucionesVentum>();
        }

        public long Id { get; set; }
        public long ConvenioId { get; set; }
        public sbyte Version { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaDatos { get; set; }
        public DateTime FechaUltimosDatos { get; set; }
        public decimal MontoNegociado { get; set; }
        public string Responsable { get; set; } = null!;
        public string PeriodoUtilizado { get; set; } = null!;
        public string? MotivoConvenio { get; set; }
        public string? Observaciones { get; set; }
        public string? ObservacionesVersion { get; set; }
        public string Estado { get; set; } = null!;

        public virtual Convenio Convenio { get; set; } = null!;
        public virtual ICollection<ConvenioMercado> ConvenioMercados { get; set; }
        public virtual ICollection<ConvenioPago> ConvenioPagos { get; set; }
        public virtual ICollection<ConveniosInstitucionesVentum> ConveniosInstitucionesVenta { get; set; }
    }
}
