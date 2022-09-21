using System;
using System.Collections.Generic;

namespace ConveniosApi.ServiciosAcromaxEntities
{
    public partial class CatArea
    {
        public CatArea()
        {
            SysUsuarios = new HashSet<SysUsuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Grupo { get; set; } = null!;
        public long? BonitaId { get; set; }
        public string Estado { get; set; } = null!;

        public virtual ICollection<SysUsuario> SysUsuarios { get; set; }
    }
}
