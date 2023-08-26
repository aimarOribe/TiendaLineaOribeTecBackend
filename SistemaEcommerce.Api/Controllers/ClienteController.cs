using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Api.Utilidades;
using Newtonsoft.Json;

namespace SistemaEcommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteServicio;

        public ClienteController(IClienteService clienteServicio)
        {
            _clienteServicio = clienteServicio;
        }

        [HttpGet]
        [Route("lista")]
        public async Task<IActionResult> lista()
        {
            var response = new Response<List<ClienteDto>>();

            try
            {
                response.status = true;
                response.data = await _clienteServicio.Lista();
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
        [Route("IniciarSesion")]
        public async Task<IActionResult> IniciarSesion([FromBody] LoginDto login)
        {
            var response = new Response<SesionClienteDto>();

            try
            {
                response.status = true;
                response.data = await _clienteServicio.ValidarCredenciales(login.Correo, login.Clave);
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
        public async Task<IActionResult> Guardar([FromBody] ClienteDto cliente)
        {
            var response = new Response<ClienteDto>();

            try
            {
                response.status = true;
                response.data = await _clienteServicio.Crear(cliente);
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
        public async Task<IActionResult> Editar([FromBody] ClienteDto clienteDto)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.data = await _clienteServicio.Editar(clienteDto);
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
                response.data = await _clienteServicio.Eliminar(id);
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
        [Route("CambiarClave")]
        public async Task<IActionResult> CambiarClave([FromBody] NewPasswordClienteDto newPasswordDto)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.data = await _clienteServicio.CambiarClave(newPasswordDto.idCliente, newPasswordDto.claveAntigua, newPasswordDto.claveNueva);
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
        [Route("ReestablecerClave")]
        public async Task<IActionResult> ReestablecerClave([FromBody] ResetPaswordDto resetPaswordDto)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.data = await _clienteServicio.RestablecerClave(resetPaswordDto.correo);
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
