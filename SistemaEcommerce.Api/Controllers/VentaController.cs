using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Api.Utilidades;
using System.Data;


namespace SistemaEcommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentaController : ControllerBase
    {
        private readonly IVentaService _ventaServicio;

        public VentaController(IVentaService ventaServicio)
        {
            _ventaServicio = ventaServicio;
        }

        [HttpPost]
        [Route("Registrar")]
        public async Task<IActionResult> Registrar([FromBody] VentaDto venta)
        {
            var response = new Response<VentaDto>();

            try
            {
                response.status = true;
                response.data = await _ventaServicio.Registrar(venta);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("Historial")]
        public async Task<IActionResult> Hitorial(string fechaInicio, string fechaFin, string? idTransaccion)
        {
            var response = new Response<List<VentaDto>>();

            idTransaccion = idTransaccion is null ? "" : idTransaccion;

            try
            {
                response.status = true;
                response.data = await _ventaServicio.Historial(fechaInicio, fechaFin, idTransaccion);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("HistorialCliente")]
        public async Task<IActionResult> HitorialCliente(string fechaInicio, string fechaFin, int idCliente, string? idTransaccion)
        {
            var response = new Response<List<VentaDto>>();

            idTransaccion = idTransaccion is null ? "" : idTransaccion;

            try
            {
                response.status = true;
                response.data = await _ventaServicio.HistorialCliente(fechaInicio, fechaFin, idCliente, idTransaccion);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("Reporte")]
        public async Task<IActionResult> Reporte(string? fechaInicio, string? fechaFin)
        {
            var response = new Response<List<ReporteDto>>();

            try
            {
                response.status = true;
                response.data = await _ventaServicio.Reporte(fechaInicio, fechaFin);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("Ubicacion/{idDistrito}")]
        public async Task<IActionResult> Ubicacion(string idDistrito)
        {
            var response = new Response<UbicacionDto>();

            try
            {
                response.status = true;
                response.data = await _ventaServicio.Ubicacion(idDistrito);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }
    }
}
