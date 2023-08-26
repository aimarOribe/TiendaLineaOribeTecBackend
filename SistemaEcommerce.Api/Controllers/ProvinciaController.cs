using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Api.Utilidades;

namespace SistemaEcommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinciaController : ControllerBase
    {
        private readonly IProvinciaService _provinciaServicio;

        public ProvinciaController(IProvinciaService provinciaServicio)
        {
            _provinciaServicio = provinciaServicio;
        }

        [HttpGet]
        [Route("ListaPorDepartamento/{idDepartamento}")]
        public async Task<IActionResult> Lista(string idDepartamento)
        {
            var response = new Response<List<ProvinciaDto>>();

            try
            {
                response.status = true;
                response.data = await _provinciaServicio.Lista(idDepartamento);
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
