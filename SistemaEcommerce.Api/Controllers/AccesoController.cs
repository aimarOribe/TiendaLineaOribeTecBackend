using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SistemaEcommerce.Api.Utilidades;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dto;

namespace SistemaEcommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccesoController : ControllerBase
    {
        private readonly IUsuarioService _usuarioServicio;

        public AccesoController(IUsuarioService usuarioServicio)
        {
            _usuarioServicio = usuarioServicio;
        }

        [HttpPut]
        [Route("CambiarClave")]
        public async Task<IActionResult> CambiarClave([FromBody] NewPasswordDto newPasswordDto)
        {
            var response = new Response<bool>();

            try
            {
                response.status = true;
                response.data = await _usuarioServicio.CambiarClave(newPasswordDto.idUsuario, newPasswordDto.claveAntigua, newPasswordDto.claveNueva);
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
                response.data = await _usuarioServicio.RestablecerClave(resetPaswordDto.correo);
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
