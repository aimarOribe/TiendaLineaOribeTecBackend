using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dal.Repositorios.Contrato;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Bll.Servicios
{
    public class VentaService : IVentaService
    {
        private readonly IVentaRepository _ventaRepositorio;
        private readonly IGenericRepository<Detalleventa> _detalleVentaRepositorio;
        private readonly IGenericRepository<Departamento> _departamentoRepositorio;
        private readonly IGenericRepository<Provincia> _provinciaRepositorio;
        private readonly IGenericRepository<Distrito> _distritoRepositorio;
        private readonly IMapper _mapper;

        public VentaService(IVentaRepository ventaRepositorio,
            IGenericRepository<Detalleventa> detalleVentaRepositorio,
            IGenericRepository<Departamento> departamentoRepositorio,
            IGenericRepository<Provincia> provinciaRepositorio,
            IGenericRepository<Distrito> distritoRepository,
            IMapper mapper)
        {
            _ventaRepositorio = ventaRepositorio;
            _detalleVentaRepositorio = detalleVentaRepositorio;
            _departamentoRepositorio = departamentoRepositorio;
            _provinciaRepositorio = provinciaRepositorio;
            _distritoRepositorio = distritoRepository;
            _mapper = mapper;
        }

        public async Task<VentaDto> Registrar(VentaDto ventaDTO)
        {
            try
            {
                var ventaGenerada = await _ventaRepositorio.Registrar(_mapper.Map<Venta>(ventaDTO));
                if (ventaGenerada.IdVenta == 0)
                {
                    throw new TaskCanceledException("No se pudo crear");
                }
                return _mapper.Map<VentaDto>(ventaGenerada);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<VentaDto>> Historial(string fechaInicio, string fechaFin, string? idTransaccion)
        {
            IQueryable<Venta> query = await _ventaRepositorio.Consultar();
            var listaResultado = new List<Venta>();

            try
            {
                if (idTransaccion == "")
                {
                    DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                    DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));
                    listaResultado = await query.Where(v =>
                        v.FechaVenta.Value.Date >= fecha_Inicio.Date &&
                        v.FechaVenta.Value.Date <= fecha_Fin.Date
                    )
                        .Include(c => c.IdClienteNavigation)
                        .Include(dv => dv.Detalleventa)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                }
                else
                {
                    DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                    DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));
                    listaResultado = await query.Where(v =>
                        v.FechaVenta.Value.Date >= fecha_Inicio.Date &&
                        v.FechaVenta.Value.Date <= fecha_Fin.Date &&
                        v.IdTransaccion == idTransaccion
                    ).Include(c => c.IdClienteNavigation)
                    .Include(dv => dv.Detalleventa)
                    .ThenInclude(p => p.IdProductoNavigation)
                    .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return _mapper.Map<List<VentaDto>>(listaResultado);
        }

        public async Task<List<ReporteDto>> Reporte(string fechaInicio, string fechaFin)
        {
            IQueryable<Detalleventa> query = await _detalleVentaRepositorio.Consultar();
            var listaResultado = new List<Detalleventa>();
            try
            {
                DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));
                listaResultado = await query
                    .Include(p => p.IdProductoNavigation)
                    .Include(v => v.IdVentaNavigation)
                    .Where(dv =>
                        dv.IdVentaNavigation.FechaVenta.Value.Date >= fecha_Inicio.Date
                        &&
                        dv.IdVentaNavigation.FechaVenta.Value.Date <= fecha_Fin.Date
                    )
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _mapper.Map<List<ReporteDto>>(listaResultado);
        }

        public async Task<UbicacionDto> Ubicacion(string idDistrito)
        {
            var ubicacionDto = new UbicacionDto();
            try
            {
                IQueryable<Distrito> queryDistrito = await _distritoRepositorio.Consultar(di => di.IdDistrito == idDistrito);
                IQueryable<Departamento> queryDepartamento = await _departamentoRepositorio.Consultar(de => de.IdDepartamento == queryDistrito.First().IdDepartamento);
                IQueryable<Provincia> queryProvincia = await _provinciaRepositorio.Consultar(pr => pr.IdProvincia == queryDistrito.First().IdProvincia);
                string descripcionDistrito = queryDistrito.First().Descripcion;
                string descripcionDepartamento = queryDepartamento.First().Descripcion;
                string descripcionProvincia = queryProvincia.First().Descripcion;
                ubicacionDto.DescripcionDepartamento = descripcionDepartamento;
                ubicacionDto.DescripcionProvincia = descripcionProvincia;
                ubicacionDto.DescripcionDistrito = descripcionDistrito;
            }
            catch (Exception)
            {
                throw;
            }
            return ubicacionDto;
        }

        public async Task<List<VentaDto>> HistorialCliente(string fechaInicio, string fechaFin, int idCliente, string? idTransaccion)
        {
            IQueryable<Venta> query = await _ventaRepositorio.Consultar();
            var listaResultado = new List<Venta>();

            try
            {
                if (idTransaccion == "")
                {
                    DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                    DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));
                    listaResultado = await query.Where(v =>
                        v.IdClienteNavigation.IdCliente == idCliente &&
                        v.FechaVenta.Value.Date >= fecha_Inicio.Date &&
                        v.FechaVenta.Value.Date <= fecha_Fin.Date
                    )
                        .Include(c => c.IdClienteNavigation)
                        .Include(dv => dv.Detalleventa)
                        .ThenInclude(p => p.IdProductoNavigation)
                        .ToListAsync();
                }
                else
                {
                    DateTime fecha_Inicio = DateTime.ParseExact(fechaInicio, "dd/MM/yyyy", new CultureInfo("es-PE"));
                    DateTime fecha_Fin = DateTime.ParseExact(fechaFin, "dd/MM/yyyy", new CultureInfo("es-PE"));
                    listaResultado = await query.Where(v =>
                        v.IdClienteNavigation.IdCliente == idCliente &&
                        v.FechaVenta.Value.Date >= fecha_Inicio.Date &&
                        v.FechaVenta.Value.Date <= fecha_Fin.Date &&
                        v.IdTransaccion == idTransaccion
                    ).Include(c => c.IdClienteNavigation)
                    .Include(dv => dv.Detalleventa)
                    .ThenInclude(p => p.IdProductoNavigation)
                    .ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }

            return _mapper.Map<List<VentaDto>>(listaResultado);
        }
    }
}
