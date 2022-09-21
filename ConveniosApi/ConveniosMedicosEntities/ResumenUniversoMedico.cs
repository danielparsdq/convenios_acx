using System;
using System.Collections.Generic;

namespace ConveniosApi.ConveniosMedicosEntities
{
    public partial class ResumenUniversoMedico
    {
        public DateTime Periodo { get; set; }
        public long Matricula { get; set; }
        public string Medico { get; set; } = null!;
        public string Provincia { get; set; } = null!;
        public string Localidad { get; set; } = null!;
        public string? Especialidad { get; set; }
        public string Mercado { get; set; } = null!;
        public short MatAct { get; set; }
        public short MatActAcx { get; set; }
        public short YtdAct { get; set; }
        public short YtdActAcx { get; set; }
        public short TrimAct { get; set; }
        public short TrimActAcx { get; set; }
        public short MesAct { get; set; }
        public short MesActAcx { get; set; }
    }
}
