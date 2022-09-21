using System;
using System.Collections.Generic;

namespace ConveniosApi.ServiciosAcromaxEntities
{
    public partial class SysRolesUsuario
    {
        public int RolId { get; set; }
        public string UsuarioId { get; set; } = null!;
        public string Estado { get; set; } = null!;

        public virtual SysRol Rol { get; set; } = null!;
        public virtual SysUsuario Usuario { get; set; } = null!;
    }
}
