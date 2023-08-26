using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dal.Repositorios.Contrato;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Entity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEcommerce.Bll.Servicios
{
    public class DashboardService: IDashboardService
    {
        private readonly IVentaRepository _ventaRepositorio;
        private readonly IGenericRepository<Producto> _productoRepositorio;
        private readonly IGenericRepository<Cliente> _clienteRepositorio;
        private readonly IMapper _mapper;

        public DashboardService(IVentaRepository ventaRepositorio, IGenericRepository<Producto> productoRepositorio, IGenericRepository<Cliente> clienteRepositorio, IMapper mapper)
        {
            _ventaRepositorio = ventaRepositorio;
            _productoRepositorio = productoRepositorio;
            _clienteRepositorio = clienteRepositorio;
            _mapper = mapper;
        }

        private IQueryable<Venta> RetornarVentas(IQueryable<Venta> tablaVenta, int restarCantidadDias)
        {
            DateTime? ultimaFecha = tablaVenta.OrderByDescending(v => v.FechaVenta).Select(v => v.FechaVenta).First();
            ultimaFecha = ultimaFecha.Value.AddDays(restarCantidadDias);
            return tablaVenta.Where(v => v.FechaVenta.Value.Date >= ultimaFecha.Value.Date);
        }

        private async Task<int> totalVentasUltimaSemana()
        {
            int total = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();
            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = RetornarVentas(_ventaQuery, -7);
                total = tablaVenta.Count();
            }
            return total;
        }

        private async Task<string> totalIngresosUltimaSemana()
        {
            decimal resultado = 0;
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();
            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = RetornarVentas(_ventaQuery, -7);
                resultado = tablaVenta.Select(v => v.MontoTotal).Sum(v => v.Value);
            }
            return Convert.ToString(resultado, new CultureInfo("es-PE"));
        }

        private async Task<int> totalProductos()
        {
            IQueryable<Producto> _productoQuery = await _productoRepositorio.Consultar();
            int total = _productoQuery.Count();
            return total;
        }

        private async Task<int> totalClientes()
        {
            IQueryable<Cliente> _productoQuery = await _clienteRepositorio.Consultar();
            int total = _productoQuery.Count();
            return total;
        }

        private async Task<Dictionary<string, int>> VentasUltimaSemana()
        {
            Dictionary<string, int> resultado = new Dictionary<string, int>();
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();
            if (_ventaQuery.Count() > 0)
            {
                var tablaVenta = RetornarVentas(_ventaQuery, -7);
                resultado = tablaVenta
                    .GroupBy(v => v.FechaVenta.Value.Date)
                    .OrderBy(g => g.Key)
                    .Select(dv => new { fecha = dv.Key.ToString("dd/MM/yyyy"), total = dv.Count() })
                    .ToDictionary(keySelector: r => r.fecha, elementSelector: r => r.total);
            }
            return resultado;
        }

        private async Task<Dictionary<string, int?>> ProductosMasVendidosUltimaSemana()
        {
            Dictionary<string, int?> resultado = new Dictionary<string, int?>();
            IQueryable<Venta> _ventaQuery = await _ventaRepositorio.Consultar();

            if (_ventaQuery.Count() > 0)
            {
                var ventasUltimaSemana = RetornarVentas(_ventaQuery, -7);

                var productosVendidos = ventasUltimaSemana
                    .SelectMany(v => v.Detalleventa) // Asumiendo que hay una propiedad "DetallesVenta" que contiene los productos vendidos en cada venta
                    .GroupBy(detalle => detalle.IdProductoNavigation.Nombre) // Agrupar por nombre de producto
                    .Select(group => new { Producto = group.Key, Cantidad = group.Sum(detalle => detalle.Cantidad)! }) // Calcular la cantidad total de cada producto
                    .OrderByDescending(group => group.Cantidad) // Ordenar por cantidad descendente
                    .ToDictionary(keySelector: r => r.Producto, elementSelector: r => r.Cantidad);

                resultado = productosVendidos;
            }

            return resultado;
        }


        public async Task<DashboardDto> Resumen()
        {
            DashboardDto vmDashboard = new DashboardDto();
            try
            {
                vmDashboard.TotalVentas = await totalVentasUltimaSemana();
                vmDashboard.TotalIngresos = await totalIngresosUltimaSemana();
                vmDashboard.totalProductos = await totalProductos();
                vmDashboard.totalClientes = await totalClientes();
                List<VentaSemanaDto> listaVentaSemana = new List<VentaSemanaDto>();
                foreach (KeyValuePair<string, int> item in await VentasUltimaSemana())
                {
                    listaVentaSemana.Add(new VentaSemanaDto()
                    {
                        Fecha = item.Key,
                        Total = item.Value,
                    }
                    );
                }
                vmDashboard.VentasUltimaSemana = listaVentaSemana;
                List<VentaSemanaProductosDto> listaVentasProductos = new List<VentaSemanaProductosDto>();
                foreach (KeyValuePair<string, int?> item in await ProductosMasVendidosUltimaSemana())
                {
                    listaVentasProductos.Add(new VentaSemanaProductosDto()
                    {
                        Producto = item.Key,
                        Cantidad = item.Value
                    });
                }
                vmDashboard.VentasUltimaSemanaProductos = listaVentasProductos;
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return vmDashboard;
        }
    }
}
