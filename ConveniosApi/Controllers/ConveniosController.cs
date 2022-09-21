using ConveniosApi.ConveniosMedicosEntities;
using ConveniosApi.Helpers;
using ConveniosApi.Models;
using ConveniosApi.Models.ConveniosModels;
using ConveniosApi.Services;
using ConveniosApi.ServiciosAcromaxEntities;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Vml;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.Linq;

namespace ConveniosApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ConveniosController : ControllerBase
    {
        private readonly IStringLocalizer<ConveniosController> _localizer;
        private readonly convenios_medicosContext _context;
        private readonly servicios_acromaxContext _contextSa;
        public ConveniosController(convenios_medicosContext context, servicios_acromaxContext contextSa, IStringLocalizer<ConveniosController> localizer)
        {
            _localizer = localizer;
            _context = context;
            _contextSa = contextSa;
        }

        /// <summary>
        /// Lista los convenios médicos
        /// </summary>
        /// <param name="filter"></param>
        /// <param name="sortBy"></param>
        /// <param name="sort"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse>> GetConvenios(
            [FromQuery] string? filter,
            [FromQuery] string? sortBy,
            [FromQuery] string? sort,
            [FromQuery] int page = 0    
        )
        {
            try
            {
                var response = new List<ConvenioModel>();
                //Begin query
                IQueryable<ConvenioVersione> query = _context.ConvenioVersiones.Include(c => c.Convenio).Include(c => c.ConvenioMercados).Where(a => a.Estado.Equals("A"));

                //Filtro solo los de GD
                if (User.IsInRole(UserService.GD.ToString()))
                {
                    query = query.Where(c => c.Responsable == User.Identity.Name);
                }
                //Filter
                if (filter != null)
                {
                    query = query.Where(s => s.Convenio.Matricula.ToString().StartsWith(filter) || s.Convenio.Medico.ToString().StartsWith(filter));
                }

                //Dinamyc sort
                switch (sortBy)
                {
                    case "Medico":
                        if (sort == "asc")
                        {
                            query = query.OrderBy(p => p.Convenio.Medico);
                        }
                        else
                        {
                            query = query.OrderByDescending(p => p.Convenio.Medico);
                        }
                        break;
                    case "Localidad":
                        if (sort == "asc")
                        {
                            query = query.OrderBy(p => p.Convenio.Localidad);
                        }
                        else
                        {
                            query = query.OrderByDescending(p => p.Convenio.Localidad);
                        }
                        break;
                    default:
                        if (sort == "asc")
                        {
                            query = query.OrderBy(p => p.FechaInicio).ThenBy(p => p.Convenio.FechaCreacion);
                        }
                        else
                        {
                            query = query.OrderByDescending(p => p.FechaInicio).ThenByDescending(p => p.Convenio.FechaCreacion);
                        }
                        break;

                }

                //Paginate
                var paginationData = await FunctionsHelper.Paginate(query, page);

                //Crea entidades ajustadas
                foreach (var _convenioVersion in paginationData.Data)
                {
                    var _usuario = await _contextSa.SysUsuarios.FirstOrDefaultAsync(u => u.User.Equals(_convenioVersion.Responsable));
                    response.Add(new ConvenioModel(_convenioVersion, _usuario));
                }
                //Response
                return new ApiResponse()
                {
                    Total = paginationData.Total,
                    Data = response,
                    Links = FunctionsHelper.GetLinks(page, paginationData.Total, Request)
                };
            }
            catch (Exception ex)
            {
                return BadRequest(FunctionsHelper.BadRequest(ex,_localizer));
            }
        }

        /// <summary>
        /// Obtiene los detalles de un convenio con el ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}")]
        public async Task<ActionResult<ConvenioModel>> Get([FromRoute] long Id)
        {
            try
            {
                var _convenio = await _context.ConvenioVersiones.Include(u => u.Convenio).Include(c => c.ConvenioMercados).SingleOrDefaultAsync(c => c.Convenio.Id.Equals(Id) && c.Estado.Equals("A"));
                var _usuario = await _contextSa.SysUsuarios.FirstOrDefaultAsync(u => u.User.Equals(_convenio.Responsable));
                return new ConvenioModel(_convenio, _usuario);
            }
            catch(Exception ex)
            {
                return BadRequest(FunctionsHelper.BadRequest(ex, _localizer));
            }
        }

        /// <summary>
        /// Crea un convenio médico
        /// </summary>
        /// <param name="convenio"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResponse>> Post([FromBody] ConvenioModel convenio)
        {
            try
            {
                var _user = await _contextSa.SysUsuarios.FirstOrDefaultAsync(u => u.User.Equals(User.Identity.Name));
                var _covnenioVersion = new ConvenioVersione()
                {
                    Estado = "A",
                    FechaInicio = convenio.FechaInicio,
                    FechaFin = convenio.FechaFin,
                    FechaDatos = convenio.FechaDatos,
                    MontoNegociado = convenio.MontoNegociado.Value,
                    Responsable = User.Identity.Name,
                    Version = 1,
                    FechaUltimosDatos = convenio.FechaUltimosDatos,
                    PeriodoUtilizado = convenio.PeriodoUtilizado,
                    ConvenioMercados = new List<ConvenioMercado>()
                };
                foreach (var detalle in convenio.Detalles)
                {
                    _covnenioVersion.ConvenioMercados.Add(new ConvenioMercado
                    {
                        AcxAct = detalle.AcxAct??0,
                        AcxAnt = detalle.AcxAnt??0,
                        AcxUltimo = detalle.AcxUltimo??0,
                        MdoAct = detalle.MdoAct??0,
                        MdoAnt = detalle.MdoAnt??0,
                        MdoUltimo = detalle.MdoUltimo??0,
                        Objetivo = detalle.Objetivo??0,
                        Cumplimiento = detalle.Cumplimiento??0,
                        Mercado = detalle.Mercado,
                        VentaActual = detalle.VentaActual??0,
                        VentaInicial = detalle.VentaInicial??0,
                        VentaObjetivo= detalle.VentaObjetivo??0
                    });
                }
                var _convenio = new Convenio()
                {
                    Anio = convenio.Anio.Value,
                    FechaCreacion = DateTime.Now,
                    Localidad = convenio.Localidad,
                    Medico = convenio.Medico,
                    Matricula = convenio.Matricula.Value,
                    Estado = "A",
                    Responsable = User.Identity.Name,
                    TipoConvenio = convenio.TipoConvenio.Value,
                    ConvenioVersiones = new List<ConvenioVersione>() { _covnenioVersion }
                };
                await _context.Convenios.AddAsync(_convenio);
                await _context.SaveChangesAsync();

                return new ApiResponse() { Data = new ConvenioModel(_covnenioVersion, _user)  };
            }
            catch (Exception ex)
            {
                return BadRequest(FunctionsHelper.BadRequest(ex, _localizer));
            }
        }

        /// <summary>
        /// Actualiza los datos del convenio generando una nueva versión
        /// </summary>
        /// <param name="convenio"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiResponse>> Put([FromBody] ConvenioModel convenio)
        {
            try
            {
                var _user = await _contextSa.SysUsuarios.FirstOrDefaultAsync(u => u.User.Equals(User.Identity.Name));
                var _convenioActual = await _context.Convenios.Include(c => c.ConvenioVersiones.Where(cv => cv.Estado == "A")).ThenInclude(cv => cv.ConvenioMercados).FirstOrDefaultAsync(c => c.Id.Equals(convenio.Id));
                if (_convenioActual == null)
                {
                    throw new Exception("convenio_no_existe");
                }
                //Desactivo cualquier versión actual
                var maxVersion = _convenioActual.ConvenioVersiones.Where(c => c.Estado.Equals("A")).Max(c => c.Version);
                var _cvActual = _convenioActual.ConvenioVersiones.FirstOrDefault(cv => cv.Version == maxVersion);
                foreach (var _cv in _convenioActual.ConvenioVersiones)
                {
                    _cv.Estado = "I";
                }
                var _covnenioVersion = new ConvenioVersione()
                {
                    Estado = "A",
                    FechaInicio = convenio.FechaInicio,
                    FechaFin = convenio.FechaFin,
                    FechaDatos = convenio.FechaDatos,
                    MontoNegociado = convenio.MontoNegociado.Value,
                    Responsable = User.Identity.Name,
                    Version = (sbyte)(maxVersion + 1),
                    FechaUltimosDatos = convenio.FechaUltimosDatos,
                    PeriodoUtilizado = convenio.PeriodoUtilizado,
                    ConvenioMercados = new List<ConvenioMercado>()
                };
                foreach (var detalle in convenio.Detalles)
                {
                    _covnenioVersion.ConvenioMercados.Add(new ConvenioMercado
                    {
                        AcxAct = detalle.AcxAct ?? 0,
                        AcxAnt = detalle.AcxAnt ?? 0,
                        AcxUltimo = detalle.AcxUltimo ?? 0,
                        MdoAct = detalle.MdoAct ?? 0,
                        MdoAnt = detalle.MdoAnt ?? 0,
                        MdoUltimo = detalle.MdoUltimo ?? 0,
                        Objetivo = detalle.Objetivo ?? 0,
                        Cumplimiento = detalle.Cumplimiento ?? 0,
                        Mercado = detalle.Mercado,
                        VentaActual = detalle.VentaActual ?? 0,
                        VentaInicial = detalle.VentaInicial ?? 0,
                        VentaObjetivo = detalle.VentaObjetivo ?? 0
                    });
                }
                _convenioActual.ConvenioVersiones.Add(_covnenioVersion);
                _context.Convenios.Update(_convenioActual);
                await _context.SaveChangesAsync();
                return new ApiResponse() { Data = new ConvenioModel(_covnenioVersion, _user) };
            }
            catch (Exception ex)
            {
                return BadRequest(FunctionsHelper.BadRequest(ex, _localizer));
            }
        }

        /// <summary>
        /// Elimina lógicamente un convenio
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete([FromRoute] long Id)
        {
            try
            {
                var convenio = await _context.Convenios.FirstOrDefaultAsync(c => c.Id.Equals(Id) && c.Estado.Equals("A"));
                if(convenio == null)
                {
                    throw new Exception("convenio_no_existe");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(FunctionsHelper.BadRequest(ex, _localizer));
            }
        }

        /// <summary>
        /// Obtiene el histórico de un convenio
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("{Id}/history")]
        public async Task<ActionResult<ApiResponse>> GetHistory(
            [FromRoute] long Id
        )
        {
            try
            {
                var convenios = await _context.ConvenioVersiones
                    .Include(c => c.Convenio)
                    .Include(c => c.ConvenioMercados)
                    .Where(a => a.Estado.Equals("I") && a.Convenio.Id.Equals(Id))
                    .OrderByDescending(a => a.Version)
                    .ToListAsync();
                //Response
                return new ApiResponse()
                {
                    Total = convenios.Count,
                    Data = convenios
                };
            }
            catch (Exception ex)
            {
                return BadRequest(FunctionsHelper.BadRequest(ex, _localizer));
            }
        }
    }
}
