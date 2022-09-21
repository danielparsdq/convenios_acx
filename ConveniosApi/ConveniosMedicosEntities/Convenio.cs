using System;
using System.Collections.Generic;

namespace ConveniosApi.ConveniosMedicosEntities
{
    public partial class Convenio
    {
        public Convenio()
        {
            ConvenioVersiones = new HashSet<ConvenioVersione>();
        }

        public long Id { get; set; }
        public long Matricula { get; set; }
        public string Responsable { get; set; } = null!;
        public string Medico { get; set; } = null!;
        public string Localidad { get; set; } = null!;
        public DateTime FechaCreacion { get; set; }
        public short Anio { get; set; }
        public sbyte TipoConvenio { get; set; }
        public bool BloqueoVersion { get; set; }
        public string Estado { get; set; } = null!;

        public virtual ConvenioTipo TipoConvenioNavigation { get; set; } = null!;
        public virtual ICollection<ConvenioVersione> ConvenioVersiones { get; set; }
    }
}
