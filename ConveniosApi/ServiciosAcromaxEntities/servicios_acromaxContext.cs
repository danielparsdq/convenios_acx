using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ConveniosApi.ServiciosAcromaxEntities
{
    public partial class servicios_acromaxContext : DbContext
    {
        public servicios_acromaxContext()
        {
        }

        public servicios_acromaxContext(DbContextOptions<servicios_acromaxContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CatArea> CatAreas { get; set; } = null!;
        public virtual DbSet<CatCargo> CatCargos { get; set; } = null!;
        public virtual DbSet<CatDepartamento> CatDepartamentos { get; set; } = null!;
        public virtual DbSet<SysRol> SysRols { get; set; } = null!;
        public virtual DbSet<SysRolesUsuario> SysRolesUsuarios { get; set; } = null!;
        public virtual DbSet<SysUsuario> SysUsuarios { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;database=servicios_acromax;user=root;password=root;treattinyasboolean=true", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.9.1-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("latin1_swedish_ci")
                .HasCharSet("latin1");

            modelBuilder.Entity<CatArea>(entity =>
            {
                entity.ToTable("cat_areas");

                entity.HasIndex(e => e.Nombre, "nombre")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.BonitaId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("bonita_id");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("'A'")
                    .IsFixedLength();

                entity.Property(e => e.Grupo)
                    .HasMaxLength(50)
                    .HasColumnName("grupo");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<CatCargo>(entity =>
            {
                entity.ToTable("cat_cargos");

                entity.HasIndex(e => e.Nombre, "nombre")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("'A'")
                    .IsFixedLength();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<CatDepartamento>(entity =>
            {
                entity.ToTable("cat_departamentos");

                entity.HasIndex(e => e.Nombre, "nombre")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .HasColumnName("id");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("'A'")
                    .IsFixedLength();

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<SysRol>(entity =>
            {
                entity.ToTable("sys_rol");

                entity.Property(e => e.Id)
                    .HasColumnType("int(11)")
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Codigo)
                    .HasMaxLength(10)
                    .HasColumnName("codigo")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("'A'")
                    .IsFixedLength()
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("nombre")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");
            });

            modelBuilder.Entity<SysRolesUsuario>(entity =>
            {
                entity.HasKey(e => new { e.RolId, e.UsuarioId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("sys_roles_usuarios");

                entity.HasIndex(e => e.UsuarioId, "FK_sys_roles_usuarios_sys_usuarios");

                entity.Property(e => e.RolId)
                    .HasColumnType("int(11)")
                    .HasColumnName("rol_id");

                entity.Property(e => e.UsuarioId)
                    .HasMaxLength(20)
                    .HasColumnName("usuario_id")
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("'A'")
                    .IsFixedLength()
                    .UseCollation("utf8mb3_general_ci")
                    .HasCharSet("utf8mb3");

                entity.HasOne(d => d.Rol)
                    .WithMany(p => p.SysRolesUsuarios)
                    .HasForeignKey(d => d.RolId)
                    .HasConstraintName("FK_sys_roles_usuarios_sys_rol");

                entity.HasOne(d => d.Usuario)
                    .WithMany(p => p.SysRolesUsuarios)
                    .HasForeignKey(d => d.UsuarioId)
                    .HasConstraintName("FK_sys_roles_usuarios_sys_usuarios");
            });

            modelBuilder.Entity<SysUsuario>(entity =>
            {
                entity.HasKey(e => e.User)
                    .HasName("PRIMARY");

                entity.ToTable("sys_usuarios");

                entity.HasCharSet("utf8mb3")
                    .UseCollation("utf8mb3_general_ci");

                entity.HasIndex(e => e.Area, "FK_sys_usuarios_cat_areas");

                entity.HasIndex(e => e.Cargo, "FK_sys_usuarios_cat_cargos");

                entity.HasIndex(e => e.Departamento, "FK_sys_usuarios_cat_departamentos");

                entity.HasIndex(e => e.Supervisor, "FK_sys_usuarios_sys_usuarios");

                entity.Property(e => e.User)
                    .HasMaxLength(20)
                    .HasColumnName("user");

                entity.Property(e => e.Alias)
                    .HasMaxLength(200)
                    .HasColumnName("alias");

                entity.Property(e => e.Apellidos)
                    .HasMaxLength(200)
                    .HasColumnName("apellidos");

                entity.Property(e => e.Area)
                    .HasColumnType("int(11)")
                    .HasColumnName("area");

                entity.Property(e => e.BonitaId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("bonita_id");

                entity.Property(e => e.Cargo)
                    .HasColumnType("int(11)")
                    .HasColumnName("cargo");

                entity.Property(e => e.Cedula)
                    .HasMaxLength(10)
                    .HasColumnName("cedula");

                entity.Property(e => e.Celular)
                    .HasMaxLength(10)
                    .HasColumnName("celular");

                entity.Property(e => e.Ciudad)
                    .HasMaxLength(300)
                    .HasColumnName("ciudad");

                entity.Property(e => e.CodigoP360)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("codigo_p360");

                entity.Property(e => e.CodigoSigma)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("codigo_sigma");

                entity.Property(e => e.Departamento)
                    .HasColumnType("int(11)")
                    .HasColumnName("departamento");

                entity.Property(e => e.Email)
                    .HasMaxLength(500)
                    .HasColumnName("email");

                entity.Property(e => e.EmailSecundario)
                    .HasMaxLength(500)
                    .HasColumnName("email_secundario");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("'A'")
                    .IsFixedLength();

                entity.Property(e => e.EvolutionId)
                    .HasColumnType("int(11)")
                    .HasColumnName("evolution_id");

                entity.Property(e => e.FechaIngreso).HasColumnName("fecha_ingreso");

                entity.Property(e => e.FechaNacimiento).HasColumnName("fecha_nacimiento");

                entity.Property(e => e.Foto)
                    .HasMaxLength(500)
                    .HasColumnName("foto");

                entity.Property(e => e.Lineas)
                    .HasMaxLength(300)
                    .HasColumnName("lineas")
                    .HasComment("Líneas que maneja");

                entity.Property(e => e.ModificacionUser)
                    .HasColumnType("datetime")
                    .HasColumnName("modificacion_user");

                entity.Property(e => e.Nombres)
                    .HasMaxLength(200)
                    .HasColumnName("nombres");

                entity.Property(e => e.NombresCompletos)
                    .HasMaxLength(400)
                    .HasColumnName("nombres_completos");

                entity.Property(e => e.ParaReemplazo).HasColumnName("para_reemplazo");

                entity.Property(e => e.Password)
                    .HasMaxLength(200)
                    .HasColumnName("password");

                entity.Property(e => e.PasswordOffice365)
                    .HasMaxLength(350)
                    .HasColumnName("password_office365");

                entity.Property(e => e.PasswordOffice365Verificado).HasColumnName("password_office365_verificado");

                entity.Property(e => e.Planta).HasColumnName("planta");

                entity.Property(e => e.Region)
                    .HasMaxLength(10)
                    .HasColumnName("region");

                entity.Property(e => e.RequiereCambioClave)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("requiere_cambio_clave")
                    .HasDefaultValueSql("'1'");

                entity.Property(e => e.SuperAdmin)
                    .HasMaxLength(1)
                    .HasColumnName("super_admin")
                    .HasDefaultValueSql("'0'")
                    .IsFixedLength();

                entity.Property(e => e.Supervisor)
                    .HasMaxLength(20)
                    .HasColumnName("supervisor");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(10)
                    .HasColumnName("telefono");

                entity.Property(e => e.UltimoCambio)
                    .HasColumnType("datetime")
                    .ValueGeneratedOnAddOrUpdate()
                    .HasColumnName("ultimo_cambio")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.UltimoIngreso)
                    .HasColumnType("datetime")
                    .HasColumnName("ultimo_ingreso");

                entity.HasOne(d => d.AreaNavigation)
                    .WithMany(p => p.SysUsuarios)
                    .HasForeignKey(d => d.Area)
                    .HasConstraintName("FK_sys_usuarios_cat_areas");

                entity.HasOne(d => d.CargoNavigation)
                    .WithMany(p => p.SysUsuarios)
                    .HasForeignKey(d => d.Cargo)
                    .HasConstraintName("FK_sys_usuarios_cat_cargos");

                entity.HasOne(d => d.DepartamentoNavigation)
                    .WithMany(p => p.SysUsuarios)
                    .HasForeignKey(d => d.Departamento)
                    .HasConstraintName("FK_sys_usuarios_cat_departamentos");

                entity.HasOne(d => d.SupervisorNavigation)
                    .WithMany(p => p.InverseSupervisorNavigation)
                    .HasForeignKey(d => d.Supervisor)
                    .HasConstraintName("FK_sys_usuarios_sys_usuarios");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
