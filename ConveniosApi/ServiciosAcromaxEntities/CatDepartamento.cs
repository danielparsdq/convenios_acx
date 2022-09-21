using System;
using System.Collections.Generic;

namespace ConveniosApi.ServiciosAcromaxEntities
{
    public partial class CatDepartamento
    {
        public CatDepartamento()
        {
            SysUsuarios = new HashSet<SysUsuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Estado { get; set; } = null!;

        public virtual ICollection<SysUsuario> SysUsuarios { get; set; }
    }
}
