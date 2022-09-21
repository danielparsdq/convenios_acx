using System;
using System.Collections.Generic;

namespace ConveniosApi.ServiciosAcromaxEntities
{
    public partial class SysUsuario
    {
        public SysUsuario()
        {
            InverseSupervisorNavigation = new HashSet<SysUsuario>();
            SysRolesUsuarios = new HashSet<SysRolesUsuario>();
        }

        public string? Foto { get; set; }
        public string User { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Nombres { get; set; }
        public string? Apellidos { get; set; }
        public string? NombresCompletos { get; set; }
        public string? Email { get; set; }
        public string? Region { get; set; }
        public string? Cedula { get; set; }
        public string? Ciudad { get; set; }
        /// <summary>
        /// Líneas que maneja
        /// </summary>
        public string? Lineas { get; set; }
        public int? Cargo { get; set; }
        public int? Departamento { get; set; }
        public int? Area { get; set; }
        public string? Telefono { get; set; }
        public string? Celular { get; set; }
        public string? EmailSecundario { get; set; }
        public DateOnly? FechaNacimiento { get; set; }
        public DateOnly? FechaIngreso { get; set; }
        public string? Alias { get; set; }
        public string? Supervisor { get; set; }
        public short RequiereCambioClave { get; set; }
        public string SuperAdmin { get; set; } = null!;
        public short? CodigoSigma { get; set; }
        public long? CodigoP360 { get; set; }
        public string? PasswordOffice365 { get; set; }
        public bool? PasswordOffice365Verificado { get; set; }
        public DateTime? UltimoIngreso { get; set; }
        public DateTime UltimoCambio { get; set; }
        public bool ParaReemplazo { get; set; }
        public long? BonitaId { get; set; }
        public int? EvolutionId { get; set; }
        public bool Planta { get; set; }
        public string Estado { get; set; } = null!;
        public DateTime? ModificacionUser { get; set; }

        public virtual CatArea? AreaNavigation { get; set; }
        public virtual CatCargo? CargoNavigation { get; set; }
        public virtual CatDepartamento? DepartamentoNavigation { get; set; }
        public virtual SysUsuario? SupervisorNavigation { get; set; }
        public virtual ICollection<SysUsuario> InverseSupervisorNavigation { get; set; }
        public virtual ICollection<SysRolesUsuario> SysRolesUsuarios { get; set; }
    }
}
