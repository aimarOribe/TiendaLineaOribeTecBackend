using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Api.Utilidades;

namespace SistemaEcommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartamentoController : ControllerBase
    {
        private readonly IDepartamentoService _departamentoServicio;

        public DepartamentoController(IDepartamentoService departamentoServicio)
        {
            _departamentoServicio = departamentoServicio;
        }

        [HttpGet]
        [Route("lista")]
        public async Task<IActionResult> lista()
        {
            var response = new Response<List<DepartamentoDto>>();

            try
            {
                response.status = true;
                response.data = await _departamentoServicio.Lista();
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
