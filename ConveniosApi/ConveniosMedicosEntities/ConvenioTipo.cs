using System;
using System.Collections.Generic;

namespace ConveniosApi.ConveniosMedicosEntities
{
    public partial class ConvenioTipo
    {
        public ConvenioTipo()
        {
            Convenios = new HashSet<Convenio>();
        }

        public sbyte Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Detalle { get; set; }
        public string Estado { get; set; } = null!;

        public virtual ICollection<Convenio> Convenios { get; set; }
    }
}
