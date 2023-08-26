using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Api.Utilidades;

namespace SistemaEcommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistritoController : ControllerBase
    {
        private readonly IDistritoService _distritoServicio;

        public DistritoController(IDistritoService distritoServicio)
        {
            _distritoServicio = distritoServicio;
        }

        [HttpGet]
        [Route("ListaProvinciaYPorDepartamento/{idProvincia}/{idDepartamento}")]
        public async Task<IActionResult> Lista(string idProvincia, string idDepartamento)
        {
            var response = new Response<List<DistritoDto>>();

            try
            {
                response.status = true;
                response.data = await _distritoServicio.Lista(idProvincia, idDepartamento);
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
