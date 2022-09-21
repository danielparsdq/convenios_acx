using System;
using System.Collections.Generic;

namespace ConveniosApi.ConveniosMedicosEntities
{
    public partial class ResumenVenta
    {
        public DateTime Periodo { get; set; }
        public string CodCliente { get; set; } = null!;
        public string? Cliente { get; set; }
        public string? GrupoCliente { get; set; }
        public string? TipoCliente { get; set; }
        public string Mercado { get; set; } = null!;
        public decimal Venta { get; set; }
        public long Unidades { get; set; }
    }
}
