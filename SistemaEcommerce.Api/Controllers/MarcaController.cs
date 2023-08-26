using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEcommerce.Api.Utilidades;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Entity;


namespace SistemaEcommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarcaController : ControllerBase
    {
        private readonly IMarcaService _marcaServicio;

        public MarcaController(IMarcaService marcaServicio)
        {
            _marcaServicio = marcaServicio;
        }

        [HttpGet]
        [Route("lista")]
        public async Task<IActionResult> lista()
        {
            var response = new Response<List<MarcaDto>>();

            try
            {
                response.status = true;
                response.data = await _marcaServicio.Lista();
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
        [Route("ListarMarcasPorCategoria/{idCategoria:int}")]
        public async Task<IActionResult> listarMarcasPorCategoria(int idCategoria)
        {
            var response = new Response<List<MarcaDto>>();

            try
            {
                response.status = true;
                response.data = await _marcaServicio.ListarMarcasPorCategoria(idCategoria);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpPost]
        [Route("Guardar")]
        public async Task<IActionResult> Guardar([FromBody] MarcaDto marca)
        {
            var response = new Response<MarcaDto>();

            try
            {
                response.status = true;
                response.data = await _marcaServicio.Crear(marca);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("Editar")]
        public async Task<IActionResult> Editar([FromBody] MarcaDto marca)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.data = await _marcaServicio.Editar(marca);
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                return StatusCode(500, response);
            }

            return Ok(response);
        }

        [HttpDelete]
        [Route("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.data = await _marcaServicio.Eliminar(id);
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
