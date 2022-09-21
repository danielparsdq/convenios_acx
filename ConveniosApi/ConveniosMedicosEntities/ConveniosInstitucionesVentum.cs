using System;
using System.Collections.Generic;

namespace ConveniosApi.ConveniosMedicosEntities
{
    public partial class ConveniosInstitucionesVentum
    {
        public long Id { get; set; }
        public long ConvenioVersionId { get; set; }
        public string CodCliente { get; set; } = null!;

        public virtual ConvenioVersione ConvenioVersion { get; set; } = null!;
    }
}
