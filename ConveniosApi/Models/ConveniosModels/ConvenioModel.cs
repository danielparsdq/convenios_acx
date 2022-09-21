using ConveniosApi.ConveniosMedicosEntities;
using ConveniosApi.ServiciosAcromaxEntities;

namespace ConveniosApi.Models.ConveniosModels
{
    public class ConvenioModel
    {
        public ConvenioModel(ConvenioVersione? _convenioVersion, SysUsuario? _usuario)
        {

            Detalles = new List<ConveniosDetalleModel>();
            if (_convenioVersion != null)
            {
                Matricula = _convenioVersion.Convenio.Matricula;
                Medico = _convenioVersion.Convenio.Medico;
                Localidad = _convenioVersion.Convenio.Localidad;
                FechaCreacion = _convenioVersion.Convenio.FechaCreacion;
                TipoConvenio = _convenioVersion.Convenio.TipoConvenio;
                Id = _convenioVersion.Convenio.Id;

                Version = _convenioVersion.Version;
                FechaInicio = _convenioVersion.FechaInicio;
                FechaFin = _convenioVersion.FechaFin;
                FechaDatos = _convenioVersion.FechaDatos;
                FechaUltimosDatos = _convenioVersion.FechaUltimosDatos;
                MontoNegociado = _convenioVersion.MontoNegociado;
                Responsable = new UsuarioModel(_usuario);
                PeriodoUtilizado = _convenioVersion.PeriodoUtilizado;

                if(_convenioVersion.ConvenioMercados.Count > 0)
                {
                    foreach(var _detalle in _convenioVersion.ConvenioMercados)
                    {
                        Detalles.Add(new ConveniosDetalleModel(_detalle));
                    }
                }
            }
        }

        public long? Id { get; set; }
        public long? Matricula { get; set; }
        public string? Medico { get; set; }
        public string? Localidad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public short? Anio { get; set; }
        public string? Estado { get; set; }
        public sbyte? Version { get; set; }
        public sbyte? TipoConvenio { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaDatos { get; set; }
        public DateTime FechaUltimosDatos { get; set; }
        public decimal? MontoNegociado { get; set; }
        public UsuarioModel? Responsable { get; set; }
        public string? PeriodoUtilizado { get; set; }
        public List<ConveniosDetalleModel> Detalles { get; set; }
    }
}
