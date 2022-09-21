using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ConveniosApi.ConveniosMedicosEntities
{
    public partial class convenios_medicosContext : DbContext
    {
        public convenios_medicosContext()
        {
        }

        public convenios_medicosContext(DbContextOptions<convenios_medicosContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Convenio> Convenios { get; set; } = null!;
        public virtual DbSet<ConvenioMercado> ConvenioMercados { get; set; } = null!;
        public virtual DbSet<ConvenioPago> ConvenioPagos { get; set; } = null!;
        public virtual DbSet<ConvenioPagoDetalle> ConvenioPagoDetalles { get; set; } = null!;
        public virtual DbSet<ConvenioTipo> ConvenioTipos { get; set; } = null!;
        public virtual DbSet<ConvenioVersione> ConvenioVersiones { get; set; } = null!;
        public virtual DbSet<ConveniosInstitucionesVentum> ConveniosInstitucionesVenta { get; set; } = null!;
        public virtual DbSet<ResumenUniversoMedico> ResumenUniversoMedicos { get; set; } = null!;
        public virtual DbSet<ResumenVenta> ResumenVentas { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;database=convenios_medicos;user=root;password=root;treattinyasboolean=true", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.9.1-mariadb"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("latin1_swedish_ci")
                .HasCharSet("latin1");

            modelBuilder.Entity<Convenio>(entity =>
            {
                entity.ToTable("convenios");

                entity.HasIndex(e => e.TipoConvenio, "FK_convenios_convenio_tipos");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.Anio)
                    .HasColumnType("smallint(4)")
                    .HasColumnName("anio");

                entity.Property(e => e.BloqueoVersion).HasColumnName("bloqueo_version");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("'A'");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_creacion");

                entity.Property(e => e.Localidad)
                    .HasMaxLength(150)
                    .HasColumnName("localidad");

                entity.Property(e => e.Matricula)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("matricula");

                entity.Property(e => e.Medico)
                    .HasMaxLength(150)
                    .HasColumnName("medico");

                entity.Property(e => e.Responsable)
                    .HasMaxLength(20)
                    .HasColumnName("responsable");

                entity.Property(e => e.TipoConvenio)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("tipo_convenio");

                entity.HasOne(d => d.TipoConvenioNavigation)
                    .WithMany(p => p.Convenios)
                    .HasForeignKey(d => d.TipoConvenio)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_convenios_convenio_tipos");
            });

            modelBuilder.Entity<ConvenioMercado>(entity =>
            {
                entity.ToTable("convenio_mercados");

                entity.HasIndex(e => new { e.ConvenioVersionId, e.Mercado }, "convenio__version_id_mercado")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AcxAct)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("acx_act");

                entity.Property(e => e.AcxAnt)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("acx_ant");

                entity.Property(e => e.AcxUltimo)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("acx_ultimo");

                entity.Property(e => e.ConvenioVersionId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("convenio__version_id");

                entity.Property(e => e.Cumplimiento).HasColumnName("cumplimiento");

                entity.Property(e => e.MdoAct)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("mdo_act");

                entity.Property(e => e.MdoAnt)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("mdo_ant");

                entity.Property(e => e.MdoUltimo)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("mdo_ultimo");

                entity.Property(e => e.Mercado)
                    .HasMaxLength(50)
                    .HasColumnName("mercado");

                entity.Property(e => e.Objetivo).HasColumnName("objetivo");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(500)
                    .HasColumnName("observaciones");

                entity.Property(e => e.ObservacionesVersion)
                    .HasMaxLength(500)
                    .HasColumnName("observaciones_version");

                entity.Property(e => e.VentaActual)
                    .HasPrecision(20, 6)
                    .HasColumnName("venta_actual");

                entity.Property(e => e.VentaInicial)
                    .HasPrecision(20, 6)
                    .HasColumnName("venta_inicial");

                entity.Property(e => e.VentaObjetivo)
                    .HasPrecision(20, 6)
                    .HasColumnName("venta_objetivo");

                entity.HasOne(d => d.ConvenioVersion)
                    .WithMany(p => p.ConvenioMercados)
                    .HasForeignKey(d => d.ConvenioVersionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_convenio_mercados_convenio_version");
            });

            modelBuilder.Entity<ConvenioPago>(entity =>
            {
                entity.ToTable("convenio_pagos");

                entity.HasIndex(e => e.ConvenioVersionId, "FK_convenio_pagos_convenio_versiones");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.ConvenioVersionId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("convenio_version_id");

                entity.Property(e => e.Direccion)
                    .HasMaxLength(250)
                    .HasColumnName("direccion");

                entity.Property(e => e.DocumentoFactura)
                    .HasMaxLength(150)
                    .HasColumnName("documento_factura");

                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("timestamp")
                    .HasColumnName("fecha_creacion")
                    .HasDefaultValueSql("current_timestamp()");

                entity.Property(e => e.FechaUltimosDatos)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_ultimos_datos");

                entity.Property(e => e.Iva).HasColumnName("iva");

                entity.Property(e => e.MontoCalculado)
                    .HasPrecision(20, 6)
                    .HasColumnName("monto_calculado");

                entity.Property(e => e.MontoJustificado)
                    .HasPrecision(20, 6)
                    .HasColumnName("monto_justificado");

                entity.Property(e => e.MontoSolicitado)
                    .HasPrecision(20, 6)
                    .HasColumnName("monto_solicitado");

                entity.Property(e => e.RazonSocial)
                    .HasMaxLength(250)
                    .HasColumnName("razon_social");

                entity.Property(e => e.Ruc)
                    .HasMaxLength(13)
                    .HasColumnName("ruc");

                entity.Property(e => e.Solicitante)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("solicitante");

                entity.Property(e => e.SubTotal)
                    .HasPrecision(20, 6)
                    .HasColumnName("sub_total");

                entity.Property(e => e.Telefono)
                    .HasMaxLength(15)
                    .HasColumnName("telefono");

                entity.Property(e => e.Total)
                    .HasPrecision(20, 6)
                    .HasColumnName("total");

                entity.HasOne(d => d.ConvenioVersion)
                    .WithMany(p => p.ConvenioPagos)
                    .HasForeignKey(d => d.ConvenioVersionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_convenio_pagos_convenio_versiones");
            });

            modelBuilder.Entity<ConvenioPagoDetalle>(entity =>
            {
                entity.ToTable("convenio_pago_detalles");

                entity.HasIndex(e => new { e.ConvenioPagoId, e.Mercado }, "convenio_pago_id_mercado")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.AcxAct)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("acx_act");

                entity.Property(e => e.AcxAnt)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("acx_ant");

                entity.Property(e => e.AcxUltimo)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("acx_ultimo");

                entity.Property(e => e.ConvenioPagoId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("convenio_pago_id");

                entity.Property(e => e.Cumplimiento).HasColumnName("cumplimiento");

                entity.Property(e => e.CumplimientoJustificado).HasColumnName("cumplimiento_justificado");

                entity.Property(e => e.Justificacion)
                    .HasMaxLength(500)
                    .HasColumnName("justificacion");

                entity.Property(e => e.MdoAct)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("mdo_act");

                entity.Property(e => e.MdoAnt)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("mdo_ant");

                entity.Property(e => e.MdoUltimo)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("mdo_ultimo");

                entity.Property(e => e.Mercado)
                    .HasMaxLength(50)
                    .HasColumnName("mercado");

                entity.Property(e => e.MontoCalculado)
                    .HasPrecision(20, 6)
                    .HasColumnName("monto_calculado");

                entity.Property(e => e.MontoJustificado)
                    .HasPrecision(20, 6)
                    .HasColumnName("monto_justificado");

                entity.Property(e => e.MontoNegociado)
                    .HasPrecision(20, 6)
                    .HasColumnName("monto_negociado");

                entity.Property(e => e.Objetivo).HasColumnName("objetivo");

                entity.Property(e => e.VentaAlcanzada)
                    .HasPrecision(20, 6)
                    .HasColumnName("venta_alcanzada");

                entity.Property(e => e.VentaInicial)
                    .HasPrecision(20, 6)
                    .HasColumnName("venta_inicial");

                entity.Property(e => e.VentaObjetivo)
                    .HasPrecision(20, 6)
                    .HasColumnName("venta_objetivo");

                entity.HasOne(d => d.ConvenioPago)
                    .WithMany(p => p.ConvenioPagoDetalles)
                    .HasForeignKey(d => d.ConvenioPagoId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_convenio_pago_detalles_convenio_pagos");
            });

            modelBuilder.Entity<ConvenioTipo>(entity =>
            {
                entity.ToTable("convenio_tipos");

                entity.Property(e => e.Id)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("id");

                entity.Property(e => e.Detalle)
                    .HasMaxLength(250)
                    .HasColumnName("detalle");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("'A'");

                entity.Property(e => e.Nombre)
                    .HasMaxLength(50)
                    .HasColumnName("nombre");
            });

            modelBuilder.Entity<ConvenioVersione>(entity =>
            {
                entity.ToTable("convenio_versiones");

                entity.HasIndex(e => e.ConvenioId, "FK_convenio_version_convenios");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("id");

                entity.Property(e => e.ConvenioId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("convenio_id");

                entity.Property(e => e.Estado)
                    .HasMaxLength(1)
                    .HasColumnName("estado")
                    .HasDefaultValueSql("'A'");

                entity.Property(e => e.FechaDatos)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_datos");

                entity.Property(e => e.FechaFin)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_fin");

                entity.Property(e => e.FechaInicio)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_inicio");

                entity.Property(e => e.FechaUltimosDatos)
                    .HasColumnType("datetime")
                    .HasColumnName("fecha_ultimos_datos");

                entity.Property(e => e.MontoNegociado)
                    .HasPrecision(20, 6)
                    .HasColumnName("monto_negociado");

                entity.Property(e => e.MotivoConvenio)
                    .HasMaxLength(500)
                    .HasColumnName("motivo_convenio");

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(500)
                    .HasColumnName("observaciones");

                entity.Property(e => e.ObservacionesVersion)
                    .HasMaxLength(500)
                    .HasColumnName("observaciones_version");

                entity.Property(e => e.PeriodoUtilizado)
                    .HasMaxLength(3)
                    .HasColumnName("periodo_utilizado")
                    .HasDefaultValueSql("'MAT'");

                entity.Property(e => e.Responsable)
                    .HasMaxLength(20)
                    .HasColumnName("responsable");

                entity.Property(e => e.Version)
                    .HasColumnType("tinyint(4)")
                    .HasColumnName("version");

                entity.HasOne(d => d.Convenio)
                    .WithMany(p => p.ConvenioVersiones)
                    .HasForeignKey(d => d.ConvenioId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_convenio_version_convenios");
            });

            modelBuilder.Entity<ConveniosInstitucionesVentum>(entity =>
            {
                entity.ToTable("convenios_instituciones_venta");

                entity.HasIndex(e => e.ConvenioVersionId, "FK_convenios_instituciones_venta_convenio_versiones");

                entity.Property(e => e.Id)
                    .HasColumnType("bigint(20)")
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.CodCliente)
                    .HasMaxLength(20)
                    .HasColumnName("cod_cliente");

                entity.Property(e => e.ConvenioVersionId)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("convenio_version_id");

                entity.HasOne(d => d.ConvenioVersion)
                    .WithMany(p => p.ConveniosInstitucionesVenta)
                    .HasForeignKey(d => d.ConvenioVersionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_convenios_instituciones_venta_convenio_versiones");
            });

            modelBuilder.Entity<ResumenUniversoMedico>(entity =>
            {
                entity.HasKey(e => new { e.Matricula, e.Periodo, e.Mercado })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("resumen_universo_medico");

                entity.Property(e => e.Matricula)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("matricula");

                entity.Property(e => e.Periodo)
                    .HasColumnType("datetime")
                    .HasColumnName("periodo");

                entity.Property(e => e.Mercado)
                    .HasMaxLength(50)
                    .HasColumnName("mercado");

                entity.Property(e => e.Especialidad)
                    .HasMaxLength(10)
                    .HasColumnName("especialidad");

                entity.Property(e => e.Localidad)
                    .HasMaxLength(150)
                    .HasColumnName("localidad");

                entity.Property(e => e.MatAct)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("mat_act");

                entity.Property(e => e.MatActAcx)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("mat_act_acx");

                entity.Property(e => e.Medico)
                    .HasMaxLength(150)
                    .HasColumnName("medico");

                entity.Property(e => e.MesAct)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("mes_act");

                entity.Property(e => e.MesActAcx)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("mes_act_acx");

                entity.Property(e => e.Provincia)
                    .HasMaxLength(150)
                    .HasColumnName("provincia");

                entity.Property(e => e.TrimAct)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("trim_act");

                entity.Property(e => e.TrimActAcx)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("trim_act_acx");

                entity.Property(e => e.YtdAct)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ytd_act");

                entity.Property(e => e.YtdActAcx)
                    .HasColumnType("smallint(6)")
                    .HasColumnName("ytd_act_acx");
            });

            modelBuilder.Entity<ResumenVenta>(entity =>
            {
                entity.HasKey(e => new { e.CodCliente, e.Periodo, e.Mercado })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

                entity.ToTable("resumen_ventas");

                entity.Property(e => e.CodCliente)
                    .HasMaxLength(50)
                    .HasColumnName("cod_cliente");

                entity.Property(e => e.Periodo)
                    .HasColumnType("datetime")
                    .HasColumnName("periodo");

                entity.Property(e => e.Mercado)
                    .HasMaxLength(50)
                    .HasColumnName("mercado");

                entity.Property(e => e.Cliente)
                    .HasMaxLength(20)
                    .HasColumnName("cliente");

                entity.Property(e => e.GrupoCliente)
                    .HasMaxLength(50)
                    .HasColumnName("grupo_cliente");

                entity.Property(e => e.TipoCliente)
                    .HasMaxLength(50)
                    .HasColumnName("tipo_cliente");

                entity.Property(e => e.Unidades)
                    .HasColumnType("bigint(20)")
                    .HasColumnName("unidades");

                entity.Property(e => e.Venta)
                    .HasPrecision(20, 6)
                    .HasColumnName("venta");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
