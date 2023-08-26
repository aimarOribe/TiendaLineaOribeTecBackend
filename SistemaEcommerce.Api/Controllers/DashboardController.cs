using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SistemaEcommerce.Bll.Servicios.Contrato;
using SistemaEcommerce.Dto;
using SistemaEcommerce.Api.Utilidades;

namespace SistemaEcommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardServicio;

        public DashboardController(IDashboardService dashboardServicio)
        {
            _dashboardServicio = dashboardServicio;
        }

        [HttpGet]
        [Route("Resumen")]
        public async Task<IActionResult> Resumen()
        {
            var response = new Response<DashboardDto>();

            try
            {
                response.status = true;
                response.data = await _dashboardServicio.Resumen();
            }
            catch (Exception ex)
            {
                response.status = false;
                response.msg = ex.Message;
                throw ex;
            }

            return Ok(response);
        }
    }
}
