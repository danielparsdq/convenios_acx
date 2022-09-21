using ConveniosApi.ConveniosMedicosEntities;

namespace ConveniosApi.Models.ConveniosModels
{
    public class ConveniosDetalleModel
    {
        public ConveniosDetalleModel(ConvenioMercado _detail)
        {
            if(_detail != null)
            {
                Id = _detail.Id;
                ConvenioVersionId = _detail.ConvenioVersionId;
                Mercado = _detail.Mercado;
                MdoAct = _detail.MdoAct;
                MdoAnt = _detail.MdoAnt;
                MdoUltimo = _detail.MdoUltimo;
                AcxAct = _detail.AcxAct;
                AcxAnt = _detail.AcxAnt;
                AcxUltimo = _detail.AcxUltimo;
                Objetivo = _detail.Objetivo;
                Cumplimiento = _detail.Cumplimiento;
                VentaActual = _detail.VentaActual;
                VentaInicial = _detail.VentaInicial;
                VentaObjetivo = _detail.VentaObjetivo;
            }
        }

        public long? Id { get; set; }
        public long? ConvenioVersionId { get; set; }
        public string? Mercado { get; set; }
        public short? MdoAnt { get; set; }
        public short? MdoAct { get; set; }
        public short? MdoUltimo { get; set; }
        public short? AcxAnt { get; set; }
        public short? AcxAct { get; set; }
        public short? AcxUltimo { get; set; }
        public float? Objetivo { get; set; }
        public decimal? VentaInicial { get; set; }
        public decimal? VentaObjetivo { get; set; }
        public decimal? VentaActual { get; set; }
        public float? Cumplimiento { get; set; }

    }
}
