using System;
using System.Collections.Generic;

namespace ConveniosApi.ServiciosAcromaxEntities
{
    public partial class SysRol
    {
        public SysRol()
        {
            SysRolesUsuarios = new HashSet<SysRolesUsuario>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Codigo { get; set; } = null!;
        public string Estado { get; set; } = null!;

        public virtual ICollection<SysRolesUsuario> SysRolesUsuarios { get; set; }
    }
}
